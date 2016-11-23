using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour, Observer
{
    [Tooltip("Duration in seconds of burning state before fire death")]
    public float burnDuration = 2f;
    [Tooltip("Duration in seconds of shocking state before electrical death")]
    public float shockDuration = 2f;
    [Tooltip("Duration in seconds of ragdoll state after death")]
    public float ragdollDuration = 2f;

    private Animator animator;
    private bool godMode;
    private bool dead;
    private EventName deathCause;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        animator = GetComponentInChildren<Animator>();
    }

    // sets dave on fire
    private void Burn()
    {
        // start animations
        animator.SetTrigger("Burn");
        Invoke("StartDeathAnimation", burnDuration);
        ThrowDeathEvent();
    }

    // sets dave electric
    private void Shock()
    {
        // start animations
        animator.SetTrigger("Shock");
        Invoke("StartDeathAnimation", shockDuration);
        ThrowDeathEvent();
    }

    private void Choke()
    {
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
        evt.payload.Add(PayloadConstants.DEATH_CAUSE, deathCause);
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
                    deathCause = EventName.OnFire;
                    Burn();
                }
                    
                break;
            //case EventName.Extinguish:
            //    onFire = false;
            //    StopCoroutine(BurnToDeath());
            //    var ExtinguishEvent = new ObserverEvent(EventName.UpdateStatus);
            //    ExtinguishEvent.payload.Add(PayloadConstants.STATUS, "");
            //    Subject.instance.Notify(gameObject, ExtinguishEvent);
            //    break;
            //case EventName.Crushed:
            //    if (!godMode)
            //        Kill(evt.eventName);
            //    break;
            case EventName.Electrocuted:
                if (!dead && !godMode)
                {
                    deathCause = EventName.Electrocuted;
                    Shock();
                }
                break;
            case EventName.PlayerExploded:
                if (!dead && !godMode)
                {
                    deathCause = EventName.PlayerExploded;
                    Burn();
                }
                //Kill(evt.eventName);
                //var explosionEvent = new ObserverEvent(EventName.OnFire);
                //Subject.instance.Notify(gameObject, explosionEvent);
                break;
            case EventName.OxygenEmpty:
                if (!dead && !godMode)
                {
                    deathCause = EventName.OxygenEmpty;
                    Choke();
                }
                    
                break;
            //case EventName.PlayerDead:
            //    gameIsOver = true;
            //    if (onFire)
            //    {
            //        var statusEvent = new ObserverEvent(EventName.Extinguish);
            //        Subject.instance.Notify(gameObject, statusEvent);
            //    }
            //    break;
            //case EventName.PlayerWon:
            //    gameIsOver = true;
            //    if (onFire)
            //    {
            //        var statusEvent = new ObserverEvent(EventName.Extinguish);
            //        Subject.instance.Notify(gameObject, statusEvent);
            //    }
            //    break;
            case EventName.GodMode:
                godMode = !godMode;
                GetComponent<OxygenController>().godMode = godMode;
                //if godmode is activated while player in on fire, extinguish
                //if (onFire)
                //{
                //    var statusEvent = new ObserverEvent(EventName.Extinguish);
                //    Subject.instance.Notify(gameObject, statusEvent);
                //}
                break;
            default:
                break;
        }
    }
}
