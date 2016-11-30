using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStage1 : MonoBehaviour, Observer
{
    public GameObject player;
    public GameObject playerCamera;
    [Header("Triggers")]
    public TutorialTrigger missingKeysTrigger;
    public GameObject aimTrigger;
    [Header("Cameras")]
    public SimpelAnimation missingKeysCamera;
    [Header("UI Tips")]
    public GameObject rotationTip;
    public GameObject powerTip;
    public GameObject launchTip;

    public Brain gal;

    private GameObject key;
    private bool hasLaunched;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, player);
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
        launchTip.SetActive(false);
        // call once player hits trigger
        missingKeysTrigger.callback = BeginMissingKeysCutscene;

        //guidanceObject.Activate();
        gal.Narrate("narrative3");
    }
    
    private void BeginMissingKeysCutscene()
    {
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
        aimTrigger.SetActive(true);
        rotationTip.SetActive(true);
        // periodically check if player aims correctly
        InvokeRepeating("CheckAim", 1f, 0.5f);
    }

    // check if the player aims correctly
    private void CheckAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;

        // Create layermask that ignores all Golfball and Ragdoll layers
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int finalmask = ~(layermask1 | layermask2);

        if (Physics.Raycast(ray, out hit, float.MaxValue, finalmask))
        {
            if (hit.collider.gameObject == aimTrigger)
            {
                // stop periodically check for aim
                CancelInvoke();
                aimTrigger.SetActive(false);
                AimSuccess();
            }
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

    void OnTriggerEnter(Collider coll)
    {
        //if (coll.CompareTag("Tutorial Room"))
        //{
        //    coll.gameObject.SetActive(false);
        //    SetStaticCamera();
        //    GetComponent<Rigidbody>().velocity = new Vector3(7.5f, 0, 0);
        //}
        //else if (coll.CompareTag("Tutorial Door"))
        //{
        //    coll.gameObject.SetActive(false);
        //    SetDoorCamera();
        //    Invoke("ToggleKeyTrigger", 2.5f);
        //}
        //else if (coll.CompareTag("Tutorial Trigger"))
        //{
        //    coll.gameObject.SetActive(false);
        //    key.gameObject.SetActive(true);
        //    Invoke("StartMovingCamera", 2.5f);
        //    GetComponent<PlayerController>().ReadyForLaunch();
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;
        //    //animator.SetTrigger("Missing Keys");
        //    var statusEvent = new ObserverEvent(EventName.DisableInput);
        //    Subject.instance.Notify(gameObject, statusEvent);
        //}
    }



    // Returns the pixel center of the camera.
    private Vector2 ScreenCenter()
    {
        return new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);
    }
    /*
    private void StopSoundEvent(string eventName, float fadeout)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        int fadeoutMs = (int)fadeout * 1000;
        AkSoundEngine.ExecuteActionOnEvent(
            eventID,
            AkActionOnEventType.AkActionOnEventType_Stop,
            gameObject, fadeoutMs,
            AkCurveInterpolation.
            AkCurveInterpolation_Sine);
    }
    */
    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
        {
            case EventName.LaunchPowerChanged:
                if (hasLaunched)
                    return;
                float force = ((Vector2)evt.payload[PayloadConstants.LAUNCH_FORCE]).x;
                bool showLaunchTip = force > 0;
                powerTip.SetActive(!showLaunchTip);
                launchTip.SetActive(showLaunchTip);
                break;
            case EventName.PlayerLaunch:
                hasLaunched = true;
                powerTip.SetActive(false);
                launchTip.SetActive(false);
                break;
            case EventName.PlayerWon:
                SceneManager.LoadScene("TutStage02");
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
