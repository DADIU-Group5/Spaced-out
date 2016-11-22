using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour, Observer
{

    PlayerController playerController;

    // Use this for initialization
    void Start () {
        Subject.instance.AddObserver(this);
        playerController = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [HideInInspector]
    public bool godMode = false;
    [HideInInspector]
    public bool onFire;
    [HideInInspector]
    public bool exploded = false;
    [HideInInspector]
    public bool electrocuted = false;
    [HideInInspector]
    public bool dead = false;

    private PayloadConstants payload;

    [Tooltip("Set the time for animations to play before Game Over:")]
    public float timeBeforeDeathScreen = 2f;

    [Tooltip("Time until burn death:")]
    public float TimeUntilBurnToDeath = 5f;

    [Tooltip("How many jumps does it take to extinguish?")]
    public int JumpsToExtinguish = 2;
    public int bounces = 0;

    private bool gameIsOver = false;


    void OnCollisionEnter(Collision other)
    {
    }

    IEnumerator SendKillNotification(EventName causeOfDeath)
    {
        Debug.Log("kill function is waiting");
        yield return new WaitForSeconds(2f);
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

    internal void Kill(EventName causeOfDeath)
    {
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);

        Debug.Log("kill function was called");
        if (!dead && !gameIsOver)
        {      
            Debug.Log("Killing player!");
            StartCoroutine(SendKillNotification(causeOfDeath));    
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.OnFire:
                if (!onFire && !gameIsOver && !godMode)
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
                if (!godMode)
                    Kill(evt.eventName);
                break;
            case EventName.Electrocuted:
                if (!electrocuted && !godMode)
                {
                    electrocuted = true;
                    ElectrocutingToDeath();
                }
                break;
            case EventName.PlayerExploded:
                if (!godMode)
                    exploded = true;
                //Kill(evt.eventName);
                var explosionEvent = new ObserverEvent(EventName.OnFire);
                Subject.instance.Notify(gameObject, explosionEvent);
                break;
            case EventName.OxygenEmpty:
                if (!godMode)
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
            case EventName.GodMode:
                godMode = !godMode;
                GameObject.FindObjectOfType<OxygenController>().godMode = godMode;
                //if godmode is activated while player in on fire, extinguish
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
            if (exploded)
            {
                Kill(EventName.PlayerExploded);
            } else
                Kill(EventName.OnFire);
            playerController.BurningToDeath();
        }
    }

    public void ElectrocutingToDeath()
    {
        //yield return new WaitForSeconds(TimeUntilBurnToDeath);
        if (electrocuted)
        {
            Debug.Log("Player has been electrocuted to death!");
            Kill(EventName.Electrocuted);
            playerController.ElectrocutedToDeath();
        }
    }

}
