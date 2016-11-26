using UnityEngine;
using System.Collections;

/// <summary>
/// This class is responsible for logic regarding player deaths and win. This involves animations and events.
/// </summary>
public class PlayerBehaviour : MonoBehaviour, Observer
{
    [Tooltip("Duration in seconds of burning state before fire death")]
    public float burnDuration = 2f;
    [Tooltip("Duration in seconds of shocking state before electrical death")]
    public float shockDuration = 2f;
    [Tooltip("Duration in seconds of ragdoll state after death")]
    public float ragdollDuration = 3f;

    private Animator animator;
    private bool godMode;
    private bool dead;
    private EventName causeOfDeath;


    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        animator = GetComponentInChildren<Animator>();
    }
    
    private void Burn()
    {
        dead = true;
        // slow player
        GetComponent<Rigidbody>().velocity *= 0.4f;
        // start animations
        animator.SetTrigger("Burn");
        Invoke("StartDeathAnimation", burnDuration);
        ThrowDeathEvent();
    }
    
    private void Shock()
    {
        dead = true;
        // slow player
        GetComponent<Rigidbody>().velocity *= 0f;
        // start animations
        animator.SetTrigger("Shock");
        Invoke("StartDeathAnimation", shockDuration);
        ThrowDeathEvent();
    }

    private void Choke()
    {
        dead = true;
        animator.SetTrigger("Choke");
        ThrowDeathEvent();
    }

    // starts the death animation in the animator
    private void StartDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    // called by animation blender when the death animation is finished (wierd workaround by bjørn)
    public void DeathAnimationOver()
    {
        // enable ragdoll
        GetComponentInChildren<RagdollAnimationBlender>().EnableRagdoll();
        // throw respawn event after small delay
        Invoke("ThrowRespawnEvent", ragdollDuration);
    }

    private void ThrowDeathEvent()
    {
        var evt = new ObserverEvent(EventName.PlayerDead);
        evt.payload.Add(PayloadConstants.DEATH_CAUSE, causeOfDeath);
        Subject.instance.Notify(gameObject, evt);
    }

    private void ThrowRespawnEvent()
    {
        var evt = new ObserverEvent(EventName.RespawnPlayer);
        Subject.instance.Notify(gameObject, evt);
        Destroy(gameObject);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerFuelPickup:
            case EventName.ComicPickup:
                animator.SetTrigger("Pick Up");
                break;
            case EventName.OnFire:
                if (!dead && !godMode)
                {
                    causeOfDeath = EventName.OnFire;
                    Burn();
                }
                break;
            case EventName.Electrocuted:
                if (!dead && !godMode)
                {
                    causeOfDeath = EventName.Electrocuted;
                    Shock();
                }
                break;
            case EventName.PlayerExploded:
                if (!dead && !godMode)
                {
                    causeOfDeath = EventName.PlayerExploded;
                    Burn();
                }
                break;
            case EventName.OxygenEmpty:
                if (!dead && !godMode)
                {
                    causeOfDeath = EventName.OxygenEmpty;
                    Choke();
                }
                break;
            case EventName.GodMode:
                godMode = !godMode;
                GetComponent<OxygenController>().godMode = godMode;
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            // in case we hit a key we throw win event and destroy key
            var evt = new ObserverEvent(EventName.PlayerGotKey);
            Subject.instance.Notify(gameObject, evt);
            //Destroy(other.gameObject);
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
