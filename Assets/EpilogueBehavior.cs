using UnityEngine;
using System.Collections;
using System;

public class EpilogueBehavior : MonoBehaviour, Observer {
    public DrawTrajectory trajectory;
    public GameObject player;

    [Header("Animations")]
    public EntryCutScene ECS; // entry through door
    public SimpelAnimation keyZoomAnimation; // zoom towards key



    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        var evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(player);
    }

    private void PlayKeyZoomAnimation()
    {
        trajectory.gameObject.SetActive(false);
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);

        keyZoomAnimation.gameObject.SetActive(true);
        keyZoomAnimation.PlayAnimations(ReturnPlayerControl);
    }

    private void ReturnPlayerControl()
    {
        trajectory.gameObject.SetActive(true);
        keyZoomAnimation.gameObject.SetActive(false);
        var evt = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, evt);
    }

    private void PlayKeyCollectedAnimation()
    {
        // stop dave
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;



        print("Collect");
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
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
