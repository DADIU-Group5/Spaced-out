using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FireHazard : MonoBehaviour, Observer
{
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            triggered = true;
            var evt = new ObserverEvent(EventName.OnFire);
            Subject.instance.Notify(gameObject, evt);
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.PlayerSpawned)
        {
            triggered = false;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
