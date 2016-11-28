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
    /// Start exploding and initiate flashing
    /// </summary>
    public IEnumerator Exploder()
    {
        //start flashing
        //StartCoroutine(Flasher());

        //wait...
        yield return new WaitForSeconds(timeToExplode);

        var evtExp = new ObserverEvent(EventName.BarrelExplosion);
        Subject.instance.Notify(gameObject, evtExp);


        //Explode, and destroy object/start animation:

        //find all objects in radius of child's spherecollider...
        Collider[] explosionObjects = Physics.OverlapSphere(transform.position, explosionRadius);
        List<Collider> filterList = new List<Collider>();//(explosionObjects);

        RaycastHit hitInfo;
        foreach (Collider collider in explosionObjects)
        {
           bool hit =  Physics.Raycast(transform.position, (collider.transform.position - transform.position), out hitInfo, explosionRadius);

            if (hit && hitInfo.transform.tag != "Respawn")
            {
                filterList.Add(collider);
            }
            else if (!hit)
            {
                filterList.Add(collider);
            }
            
        }

        Collider[] filtered = filterList.ToArray();

        for (int i = 0; i < filtered.Length; i++)
        {
            Rigidbody rgb = filtered[i].GetComponent<Rigidbody>();
            if (rgb == null)
                continue;
            if (filtered[i].tag == "Player")
            {
                Debug.Log("player collider tag found: " + filtered[i].name);
                var evt = new ObserverEvent(EventName.PlayerExploded);
                Subject.instance.Notify(gameObject, evt);
            }
        }
        filterList.Clear();

        Collider[] pushObjects = Physics.OverlapSphere(transform.position, pushRadius);
        Debug.Log("pushobjects: " + pushObjects.Length);
        foreach (Collider collider in pushObjects)
        {
            bool hit = Physics.Raycast(transform.position, (collider.transform.position - transform.position), out hitInfo, explosionRadius);

            if (hit && hitInfo.transform.tag != "Respawn")
            {
                filterList.Add(collider);
            }
            else if (!hit)
            {
                filterList.Add(collider);
            }
        }

        pushObjects = filterList.ToArray();

        for (int i = 0; i < pushObjects.Length; i++)
        {
            Rigidbody rgb = pushObjects[i].GetComponent<Rigidbody>();
            if (rgb == null)
                continue;
            //and Boom!

            // transform.position should be pushDirection - check results.
            rgb.AddExplosionForce(explosionPower, transform.position, pushRadius);
        }

        Destroy(gameObject);
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
            if (other.transform.tag == "Player" ||
            other.transform.tag == "object" && other.gameObject.GetComponent<Rigidbody>() != null)
            {
                var evt = new ObserverEvent(EventName.BarrelTriggered);
                Subject.instance.Notify(gameObject, evt);

                pushForce = other.rigidbody.velocity.magnitude;
                pushDirection = other.contacts[0].point - transform.position;
                pushDirection = -pushDirection.normalized;

                barrelTrigger.TriggerBarrel();

                //if the velocity is enough to explode...
                if (pushForce >= explodeForce)
                {
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
