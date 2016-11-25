using UnityEngine;
using System.Collections;

public class EpilogueDoor : MonoBehaviour, Observer {

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    GameObject player;

	public void OnNotify(GameObject go, ObserverEvent evt)
    {
        if(evt.eventName == EventName.EPILOGUE_EVENTONE)
        {
            player = go;
            player.GetComponent<Rigidbody>().velocity = new Vector3(0.001f, 0, 0);
            GetComponent<Animator>().SetTrigger("Open");
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void Ended()
    {
        //var evt = new ObserverEvent(EventName.ToggleUI);
        //Subject.instance.Notify(gameObject, evt);

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, player.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
        player.GetComponent<Rigidbody>().isKinematic = false;
    }
}