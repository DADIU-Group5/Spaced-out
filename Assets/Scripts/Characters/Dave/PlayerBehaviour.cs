﻿using UnityEngine;
using System.Collections;

// my thought was that this script could be used for
// collisions, and player behaviour.
// input/movement would be a separate script.
public class PlayerBehaviour : MonoBehaviour, Observer
{

    Rigidbody rgb;
    //[HideInInspector]
    public bool onFire;
    [HideInInspector]
    public bool dead = false;

    private PayloadConstants payload;

    [Tooltip("Time until burn death:")]
    public float TimeUntilBurnToDeath = 5f;

    [Tooltip("How many jumps does it take to extinguish?")]
    public int JumpsToExtinguish = 2;
    public int bounces = 0;

    void Start()
    {
        rgb = this.gameObject.GetComponent<Rigidbody>();
        Subject.instance.AddObserver(this);
    }

    void OnCollisionEnter(Collision other)
    {
        if (onFire)
        {
            bounces += 1;
            if (bounces >= JumpsToExtinguish)
            {
                Debug.Log("Extinguishing");
                bounces = 0;
                var evt = new ObserverEvent(EventName.Extinguish);
                Subject.instance.Notify(gameObject, evt);
            }
        }
    }

    internal void Kill(EventName causeOfDeath)
    {
        if (!dead)
        {
            var evt = new ObserverEvent(EventName.PlayerDead);
            evt.payload.Add(PayloadConstants.DEATH_CAUSE, causeOfDeath);
            Subject.instance.Notify(gameObject, evt);
            dead = true;
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.OnFire:
                if (!onFire)
                {
                    onFire = true;

                    StartCoroutine(BurnToDeath());

                    var statusEvent = new ObserverEvent(EventName.UpdateStatus);
                    statusEvent.payload.Add(PayloadConstants.STATUS, "BURNING!");
                    Subject.instance.Notify(gameObject, statusEvent);
                }
                break;
            case EventName.Extinguish:
                onFire = false;
                Debug.Log("Not on fire anymore!");
                StopCoroutine(BurnToDeath());
                var ExtinguishEvent = new ObserverEvent(EventName.UpdateStatus);
                ExtinguishEvent.payload.Add(PayloadConstants.STATUS, "");
                Subject.instance.Notify(gameObject, ExtinguishEvent);
                break;
            case EventName.Crushed:
                Kill(evt.eventName);
                //Kill(EventName.Crushed);
                break;
            case EventName.Electrocuted:
                Kill(evt.eventName);
                //Kill(EventName.Electrocuted);
                break;
            case EventName.PlayerExploded:
                Kill(evt.eventName);
                //Kill(EventName.PlayerExploded);
                break;
            case EventName.FuelEmpty:
                Kill(evt.eventName);
                //Kill(EventName.FuelEmpty);
                break;
            default:
                break;
        }
    }

    public IEnumerator BurnToDeath()
    {
        Debug.Log("burning coroutine");
        yield return new WaitForSeconds(TimeUntilBurnToDeath);

        //if the player is still on fire after this time, die.
        if (onFire)
        {
            Debug.Log("Player has burned to death!");
            Kill(EventName.OnFire);
        }
    }
}
