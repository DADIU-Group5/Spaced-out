using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OxygenController))]
public class MovementBehaviour : MonoBehaviour
{
    // For ensuring that the player at some point starts slowing
    [Tooltip("Player will stop ragdolling when moving slower than this threshold")]
    public float ragdollThreshold = 4;
    [Tooltip("Player is slowed down when moving slower than this threshold")]
    public float slowThreshold = 2;
    [Tooltip("Player is stopped when moving slower than this threshold")]
    public float stopThreshold = 0.2f;
    [Tooltip("How fast will the player be slowed down")]
    [Range(0, 1)]
    public float slowFactor = 0.02f;

    private bool canSlowDown;
    private Rigidbody body;
    private OxygenController oxygen;
    private PlayerController playerController;
    private RagdollAnimationBlender animationBlender;
    private bool ragdolling;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        oxygen = GetComponent<OxygenController>();
        playerController = GetComponent<PlayerController>();
        animationBlender = GetComponentInChildren<RagdollAnimationBlender>();
    }

    void LateUpdate()
    {
        // do nothing if player is not moving
        if (body.velocity == Vector3.zero)
            return;

        // disable ragdoll
        if (ragdolling && body.velocity.magnitude < ragdollThreshold)
        {
            animationBlender.DisableRagdoll();
            ragdolling = false;
        }

        // slow down player
        if (body.velocity.magnitude < slowThreshold)
        {
            body.velocity = body.velocity * (1f - slowFactor);
        }

        // stop player
        if (body.velocity.magnitude < stopThreshold)
        {
            body.velocity = Vector3.zero;
            playerController.ReadyForLaunch();
        }
    }

    private void OnCollisionEnter()
    {
        // enable ragdoll
        print("Velocity: " + body.velocity.magnitude);
        if (body.velocity.magnitude > ragdollThreshold)
        {
            animationBlender.EnableRagdoll();
            ragdolling = true;
        }
    }


    //[HideInInspector]
    //public bool onFire;
    //public bool electrocuted = false;
    //[HideInInspector]
    //public bool dead = false;

    //private PayloadConstants payload;

    //[Tooltip("Time until burn death:")]
    //public float TimeUntilBurnToDeath = 5f;

    //[Tooltip("How many jumps does it take to extinguish?")]
    //public int JumpsToExtinguish = 2;
    //public int bounces = 0;

    //private bool gameIsOver = false;


    //void OnCollisionEnter(Collision other)
    //{
    //    if (onFire)
    //    {
    //        bounces += 1;
    //        if (bounces >= JumpsToExtinguish)
    //        {
    //            Debug.Log("Extinguishing");
    //            bounces = 0;
    //            var evt = new ObserverEvent(EventName.Extinguish);
    //            Subject.instance.Notify(gameObject, evt);
    //        }
    //    }
    //}

    //internal void Kill(EventName causeOfDeath)
    //{
    //    if (!dead && !gameIsOver)
    //    {
    //        Debug.Log("Killing player!");
    //        var evt = new ObserverEvent(EventName.PlayerDead);
    //        evt.payload.Add(PayloadConstants.DEATH_CAUSE, causeOfDeath);
    //        dead = true;

    //        Subject.instance.Notify(gameObject, evt);

    //        //Actual death.
    //        if (transform.parent != null)
    //        {
    //            transform.parent.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            transform.gameObject.SetActive(false);
    //        }
    //    }
    //}

    //public void OnNotify(GameObject entity, ObserverEvent evt)
    //{
    //    switch (evt.eventName)
    //    {
    //        case EventName.OnFire:
    //            if (!onFire && !gameIsOver)
    //            {
    //                onFire = true;

    //                StartCoroutine(BurnToDeath());

    //                var statusEvent = new ObserverEvent(EventName.UpdateStatus);
    //                statusEvent.payload.Add(PayloadConstants.STATUS, "BURNING!");
    //                Subject.instance.Notify(gameObject, statusEvent);
    //            }
    //            break;
    //        case EventName.Extinguish:
    //            onFire = false;
    //            Debug.Log("Not on fire anymore!");
    //            StopCoroutine(BurnToDeath());
    //            var ExtinguishEvent = new ObserverEvent(EventName.UpdateStatus);
    //            ExtinguishEvent.payload.Add(PayloadConstants.STATUS, "");
    //            Subject.instance.Notify(gameObject, ExtinguishEvent);
    //            break;
    //        case EventName.Crushed:
    //            Kill(evt.eventName);
    //            break;
    //        case EventName.Electrocuted:
    //            if (!electrocuted)
    //            {
    //                electrocuted = true;
    //                Kill(evt.eventName);
    //            }
    //            break;
    //        case EventName.PlayerExploded:
    //            Kill(evt.eventName);
    //            break;
    //        case EventName.OxygenEmpty:
    //            Kill(evt.eventName);
    //            break;
    //        case EventName.PlayerDead:
    //            gameIsOver = true;
    //            PlayerPrefs.SetInt("playerDiedThisLevel", 1);
    //            if (onFire)
    //            {
    //                var statusEvent = new ObserverEvent(EventName.Extinguish);
    //                Subject.instance.Notify(gameObject, statusEvent);
    //            }
    //            break;
    //        case EventName.PlayerWon:
    //            gameIsOver = true;
    //            if (onFire)
    //            {
    //                var statusEvent = new ObserverEvent(EventName.Extinguish);
    //                Subject.instance.Notify(gameObject, statusEvent);
    //            }
    //            break;
    //        default:
    //            break;
    //    }
    //}

    //public IEnumerator BurnToDeath()
    //{
    //    Debug.Log("burning coroutine");
    //    yield return new WaitForSeconds(TimeUntilBurnToDeath);

    //    //if the player is still on fire after this time, die.
    //    if (onFire)
    //    {
    //        Debug.Log("Player has burned to death!");
    //        Kill(EventName.OnFire);
    //    }
    //}


}
