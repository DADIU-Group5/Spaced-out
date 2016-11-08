using UnityEngine;
using System.Collections;

// my thought was that this script could be used for
// collisions, and player behaviour.
// input/movement would be a separate script.
public class PlayerBehaviour : MonoBehaviour, Observer
{

    Rigidbody rgb;
    [HideInInspector]
    public bool onFire;
    [HideInInspector]
    public bool dead = false;

    public float TimeUntilBurnToDeath = 5f;

    void Start()
    {
        rgb = this.gameObject.GetComponent<Rigidbody>();
        Subject.instance.AddObserver(this);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "object")
        {
            PlayerMetObject(other.gameObject);
        }
    }

    /// <summary>
    /// Player met and object, decide on proper reaction.
    /// </summary>
    public void PlayerMetObject(GameObject obj)
    {
        Behaviour objBehaviour = obj.GetComponent<GameplayElement>().behaviour;
        switch (objBehaviour)
        {
            case Behaviour.electrocution:
                Debug.Log("Hair-raising!");
                return;
            
            default:
                return;
        }
    }

    internal void Kill()
    {
        dead = true;
        var evt = new ObserverEvent(EventName.PlayerDead);
        Subject.instance.Notify(gameObject, evt);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.OnFire:
                onFire = true;
                StartCoroutine(BurnToDeath());
                break;
            case EventName.Extinguish:
                onFire = false;
                break;
            case EventName.Crushed:
                Kill();
                break;
            case EventName.Electrocuted:
                Kill();
                break;
            case EventName.PlayerExploded:
                Kill();
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
            Kill();
        }
    }

   /* void Update()
    {
        if ()
        {
            onFire = false;
        }
    }*/
}
