using UnityEngine;

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
    }

    private void PlayKeyCollectedAnimation()
    {
        // play key pick up sound
        AkSoundEngine.PostEvent("pickupKeys", gameObject);

        // stop dave
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("Key").SetActive(false);
        player.GetComponentInChildren<Animator>().SetTrigger("Pick Up");
        Invoke("Idle", 0.1f);
    }

    private void Idle()
    {
        player.GetComponentInChildren<Animator>().SetTrigger("Ready To Launch");
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
