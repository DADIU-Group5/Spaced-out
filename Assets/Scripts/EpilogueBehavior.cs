using UnityEngine;
using System.Collections;

public class EpilogueBehavior : MonoBehaviour, Observer
{
    public DrawTrajectory trajectory;
    public GameObject player;
    public TutorialTrigger roomTrigger;
    public SimpelAnimation doorAnimation;

    [Header("Animations")]
    public EntryCutScene ECS; // entry through door
    public SimpelAnimation keyZoomAnimation; // zoom towards key
    public SimpelAnimation roomCamAnimation;
    public GameObject key;

    public GameObject doorLock;
    public GameObject wind;


    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        var evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(player);

        SoundManager.instance.StopMusic();

        roomTrigger.callback = RoomEntered;
    }

    // animate camera going towards key
    private void PlayKeyZoomAnimation()
    {
        trajectory.gameObject.SetActive(false);
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);

        keyZoomAnimation.gameObject.SetActive(true);
        keyZoomAnimation.PlayAnimations(ReturnPlayerControl);
    }

    // give control back to player after opening cutscene
    private void ReturnPlayerControl()
    {
        trajectory.gameObject.SetActive(true);
        keyZoomAnimation.gameObject.SetActive(false);
        roomCamAnimation.gameObject.SetActive(false);
        var evt = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, evt);
    }

    // called once the player enters the room
    private void RoomEntered()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(4f, 0, 0);
        player.transform.position = roomTrigger.transform.position;
        trajectory.gameObject.SetActive(false);
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);
        roomCamAnimation.gameObject.SetActive(true);
        roomCamAnimation.PlayAnimations(OpenExitDoor);
    }

    private void OpenExitDoor()
    {
        wind.SetActive(true);
        SoundManager.instance.StopEvent("keysAmbient", 0.5f, key);
        AkSoundEngine.PostEvent("spaceWind", gameObject);
        doorAnimation.PlayAnimations(FireDaveThroughExitDoor);
    }

    private void FireDaveThroughExitDoor()
    {
        AkSoundEngine.PostEvent("suckedIntoSpace", gameObject);
        AkSoundEngine.PostEvent("musicCredits", gameObject);
        player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 12);
        player.GetComponentInChildren<RagdollAnimationBlender>().EnableRagdoll();
        Invoke("LoadEpilogueMenu", 2.5f);
    }

    private void PlayKeyCollectedAnimation()
    {
        // play key pick up sound
        AkSoundEngine.PostEvent("pickupKeys", gameObject);

        // stop dave
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Invoke("RemoveKey", 0.75f);
        player.GetComponentInChildren<Animator>().SetTrigger("Pick Up");
        Invoke("Idle", 0.1f);
    }

    private void RemoveKey()
    {
        var evt = new ObserverEvent(EventName.PickUpKey);
        Subject.instance.Notify(key, evt);
        player.GetComponent<PlayerController>().aimRotateSpeed = 25f;
        player.GetComponent<PlayerController>().ReadyForLaunch();
        player.GetComponent<PlayerController>().Aim(doorLock.transform.position);
        key.SetActive(false);
        StartCoroutine(ScaleKey(1f, doorLock.transform));

    }

    private void LoadEpilogueMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(7);
    }

    private void Idle()
    {
        player.GetComponentInChildren<Animator>().SetTrigger("Ready To Launch");
    }

    IEnumerator ScaleKey(float t, Transform trans)
    {
        Vector3 newScale;
        while (t > 0)
        {
            t -= Time.deltaTime;

            newScale = Vector3.one * t;
            newScale.z = 1;
            trans.localScale = newScale;
            yield return new WaitForEndOfFrame();
        }
        trans.gameObject.SetActive(false);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerSpawned:
                PlayKeyZoomAnimation();
                break;
            case EventName.PlayerGotKey:
                PlayKeyCollectedAnimation();
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
