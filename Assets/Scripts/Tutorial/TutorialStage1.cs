using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStage1 : MonoBehaviour, Observer
{
    public GameObject player;
    public GameObject playerCamera;
    [Header("Triggers")]
    public TutorialTrigger missingKeysTrigger;
    [Header("Cameras")]
    public SimpelAnimation missingKeysCamera;
    [Header("UI Tips")]
    public GameObject rotationTip;
    public GameObject powerTip;

    public Brain gal;

    private GameObject key;
    private bool scaleGuide1, scaleGuide2;
    private Vector3 startDir;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);

        SoundManager.instance.StartMusic();

        gal.randomRemarks = false;

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, player);
        Subject.instance.Notify(gameObject, evt);

        // disable GAL
        evt = new ObserverEvent(EventName.ToggleGAL);
        evt.payload.Add(PayloadConstants.SWITCH_ON, false);
        Subject.instance.Notify(gameObject, evt);

        // disable key
        key = GameObject.FindGameObjectWithTag("Key");
        key.SetActive(false);
        // disable camera input
        var statusEvent = new ObserverEvent(EventName.DisableCameraInput);
        Subject.instance.Notify(gameObject, statusEvent);
        // setup UI tips
        rotationTip.SetActive(false);
        powerTip.SetActive(true);
        // call once player hits trigger
        missingKeysTrigger.callback = BeginMissingKeysCutscene;

        //guidanceObject.Activate();
        gal.Narrate("narrative3");
    }

    private void BeginMissingKeysCutscene()
    {
        powerTip.gameObject.SetActive(false);

        // spawn key
        key.SetActive(true);

        // set fixed velocity
        player.GetComponent<Rigidbody>().velocity = new Vector3(8.5f, 0, 0);
        Invoke("PlayMissingKeysAnimation", 2f);

        // begin camera animation
        playerCamera.SetActive(false);
        missingKeysCamera.gameObject.SetActive(true);
        missingKeysCamera.PlayAnimations(MissingKeysCutsceneDone);

        ToggleUI();
    }

    private void PlayMissingKeysAnimation()
    {
        player.GetComponentInChildren<Animator>().SetTrigger("Missing Keys");

    }

    // will be called once missing keys cutscene is over
    private void MissingKeysCutsceneDone()
    {
        ToggleUI();

        player.GetComponentInChildren<Animator>().ResetTrigger("Ready To Launch");
        missingKeysCamera.gameObject.SetActive(false);
        playerCamera.SetActive(true);

        var statusEvent = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, statusEvent);

        gal.Narrate("narrative7");

        // begin aim phase
        rotationTip.SetActive(true);
        // periodically check if player aims correctly
        startDir = Camera.main.ScreenPointToRay(ScreenCenter()).direction;
        InvokeRepeating("CheckAim", 1f, 0.5f);
    }

    // check if the player aims correctly
    private void CheckAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;

        // scale down aim tutorial
        if (!scaleGuide2 && startDir != ray.direction)
        {
            var rectTransform = rotationTip.GetComponent<RectTransform>();
            StartCoroutine(HideWindow(rectTransform));
            scaleGuide2 = true;
        }
    }

    // called when the player successfully aims
    private void AimSuccess()
    {
        //StopSoundEvent("narrative3", 0.5f);
        gal.Narrate("narrative5");
        rotationTip.SetActive(false);
    }

    private void ToggleUI()
    {
        var statusEvent = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, statusEvent);
    }

    // Returns the pixel center of the camera.
    private Vector2 ScreenCenter()
    {
        if(Camera.main == null)
        {
            return Vector2.zero;
        }
        return new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.LaunchPowerChanged:
                if (scaleGuide1)
                    return;

                float force = ((Vector2)evt.payload[PayloadConstants.LAUNCH_FORCE]).x;
                if (force == 0)
                {
                    var rectTransform = powerTip.GetComponent<RectTransform>();
                    StartCoroutine(HideWindow(rectTransform));
                    scaleGuide1 = true;
                }
                break;
            case EventName.PlayerWon:
                SceneManager.LoadScene("TutStage02");
                break;
            case EventName.PlayerGotKey:
                rotationTip.SetActive(false);
                break;
        }
    }

    // used to scale down a tutorial screen
    private IEnumerator HideWindow(RectTransform rect)
    {
        var curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float t = 0;
        float duration = 0.5f;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(630, 300);
        Vector3 startScale = rect.localScale;
        Vector3 endScale = new Vector3(0.4f, 0.4f, 0.4f);

        while (t < 1)
        {
            t += Time.deltaTime / duration;
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, curve.Evaluate(t));
            rect.localScale = Vector3.Lerp(startScale, endScale, curve.Evaluate(t));
            yield return null;
        }

        rect.anchoredPosition = endPos;
        rect.localScale = endScale;
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
