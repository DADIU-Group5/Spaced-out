﻿using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour, Observer
{

	// Use this for initialization
	void Start () {
        Subject.instance.AddObserver(this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [HideInInspector]
    public bool onFire;
    public bool electrocuted = false;
    [HideInInspector]
    public bool dead = false;

    private PayloadConstants payload;

    [Tooltip("Time until burn death:")]
    public float TimeUntilBurnToDeath = 5f;

    [Tooltip("How many jumps does it take to extinguish?")]
    public int JumpsToExtinguish = 2;
    public int bounces = 0;

    private bool gameIsOver = false;


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
        if (!dead && !gameIsOver)
        {
            Debug.Log("Killing player!");
            var evt = new ObserverEvent(EventName.PlayerDead);
            evt.payload.Add(PayloadConstants.DEATH_CAUSE, causeOfDeath);
            dead = true;

            Subject.instance.Notify(gameObject, evt);

            //Actual death.
            if (transform.parent != null)
            {
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.OnFire:
                Debug.Log("OnFire notification in movementbehaviour!");
                if (!onFire && !gameIsOver)
                {
                    Debug.Log("Player is not on fire...");
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
                break;
            case EventName.Electrocuted:
                if (!electrocuted)
                {
                    electrocuted = true;
                    Kill(evt.eventName);
                }
                break;
            case EventName.PlayerExploded:
                Kill(evt.eventName);
                break;
            case EventName.OxygenEmpty:
                Kill(evt.eventName);
                break;
            case EventName.PlayerDead:
                gameIsOver = true;
                PlayerPrefs.SetInt("playerDiedThisLevel", 1);
                if (onFire)
                {
                    var statusEvent = new ObserverEvent(EventName.Extinguish);
                    Subject.instance.Notify(gameObject, statusEvent);
                }
                break;
            case EventName.PlayerWon:
                gameIsOver = true;
                if (onFire)
                {
                    var statusEvent = new ObserverEvent(EventName.Extinguish);
                    Subject.instance.Notify(gameObject, statusEvent);
                }
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
