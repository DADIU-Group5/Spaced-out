using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosiveBarrelHazard : MonoBehaviour {

    //determined by playerspeed, etc.
    [HideInInspector]
    public float pushForce = 50f;

    [Header("Power of the explosion:")]
    public float explosionPower = 50f;

    [Header("In the damaging radius:")]
    public float explosionRadius = 10f;
    [Header("In the pushing radius:")]
    public float pushRadius = 50f;

    public BarrelTrigger barrelTrigger;

    //direction of the push, based on collision
    [HideInInspector]
    public Vector3 pushDirection;

    [Header("The force required to explode:")]
    [Tooltip("The force from the player at which the barrel explodes.")]
    public float explodeForce = 0.5f;

    [Header("Time between push and explosion:")]
    public float timeToExplode = 2f;

    [Tooltip("Sets the flashing color")]
    public Color detonationColor = Color.blue;

    private Color orgColor;

    [HideInInspector]
    public GameplayElement itemState;

    void Start()
    {
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }

    /// <summary>
    /// Flash to signify explosion
    /// </summary>
    public IEnumerator Flasher()
    {
        //save the original colouring
        orgColor = gameObject.GetComponent<Renderer>().material.color;
        //start flipping between colours.
        while (true)
        {
            gameObject.GetComponent<Renderer>().material.color = detonationColor;
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<Renderer>().material.color = orgColor;
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// EXPLOSION!
    /// </summary>
    /// <returns></returns>
    IEnumerator Exploder()
    {
        //TODO visual effect?

        yield return new WaitForSeconds(timeToExplode);

        var evtExp = new ObserverEvent(EventName.BarrelExplosion);
        Subject.instance.Notify(gameObject, evtExp);

        Collider[] colls = Physics.OverlapSphere(transform.position,pushRadius);

        RaycastHit hitinfo;

        List<Collider> pushColls = new List<Collider>();

        foreach (Collider item in colls)
        {
            if(!(item.transform.tag == "Player" || item.transform.tag == "Floating Object"))
            {
                continue;
            }
            if(Physics.Raycast(transform.position, item.transform.position - transform.position, out hitinfo))
            {
                if (hitinfo.transform.tag == "Player" )
                {
                    pushColls.Add(item);
                    if (hitinfo.distance < explosionRadius)
                    {
                        var evt = new ObserverEvent(EventName.PlayerExploded);
                        Subject.instance.Notify(gameObject, evt);
                    }
                }
                else if(hitinfo.transform.tag == "Floating Object")
                {
                    pushColls.Add(item);
                }
            }
        }

        foreach (Collider item in pushColls)
        {
            PushObject(item.transform);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Function for pushing objects.
    /// </summary>
    /// <param name="obj"></param>
    void PushObject(Transform obj)
    {
        if(obj.GetComponent<Rigidbody>() != null)
        {
            if (obj.tag == "Player")
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionPower * 30, transform.position, pushRadius);
            }
            else
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position, pushRadius);
            }

        }
    }

    /// <summary>
    /// The object checks if anything with a high enough velocity hits it...
    /// if it does, it explodes, and adds force to every object in radius
    /// the radius is determined by the spherecollider of the Barrel's child
    /// which also keeps track of what is in range.
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        if (itemState.On)
        {
            if (other.transform.tag == "Player")
            {
                var evt = new ObserverEvent(EventName.BarrelTriggered);
                Subject.instance.Notify(gameObject, evt);

                pushForce = other.rigidbody.velocity.magnitude;
                pushDirection = other.contacts[0].point - transform.position;
                pushDirection = -pushDirection.normalized;


                //if the velocity is enough to explode...
                if (pushForce >= explodeForce)
                {
                    barrelTrigger.TriggerBarrel();
                    StartCoroutine(Exploder());
                }
                //if the force is not enough to explode, just push instead.
                else
                {
                    GetComponent<Rigidbody>().AddForce(pushDirection * pushForce);
                }
            }
        }
    }
}
