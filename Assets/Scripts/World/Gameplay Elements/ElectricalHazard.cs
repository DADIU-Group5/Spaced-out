using UnityEngine;
using System.Collections;

public class ElectricalHazard : MonoBehaviour, Observer
{
    private bool triggered = false;

    [HideInInspector]
    public GameplayElement itemState;

    private void Awake()
    {
        itemState = GetComponent<GameplayElement>();
        Subject.instance.AddObserver(this);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.transform.tag == "Player" && itemState.On)
        {
            triggered = true;
            var evt = new ObserverEvent(EventName.Electrocuted);
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
