using UnityEngine;
using System.Collections.Generic;

public class BouncyHazard : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

    [Tooltip("Set bounciness!")]
    public float bounciness = 0.5f;

    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //so far is only adds force, like the fan does - how should this work
    void FixedUpdate()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Rigidbody rgb = objects[i].GetComponent<Rigidbody>();
            //foreach object in sticky influence, lower velocity. (Force instead?)
            rgb.AddForce(Vector3.up * bounciness, ForceMode.Impulse);
        }
    }
    /*
    public IEnumerator Bouncer()
    {

        yield return new WaitForSeconds(1f);
        //do something to the list of objects every x seconds!
        bounce;
        wait...
        bring down;
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" &&
            other.transform.GetComponent<GameplayElement>().movement == Movement.floatingItem)
        {   //if it's an object with a rigidbody (moveable),
            //add to list of objects in collider...
            objects.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" &&
            other.transform.GetComponent<GameplayElement>().movement == Movement.floatingItem)
        {   //if the objects leave the collider, remove from list.
            objects.Remove(other);
        }
    }
}
