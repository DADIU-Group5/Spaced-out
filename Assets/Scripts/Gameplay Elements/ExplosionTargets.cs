using UnityEngine;
using System.Collections.Generic;

public class ExplosionTargets : MonoBehaviour {

    // Internal list that tracks objects that enter this object's "zone"
    public List<Collider> objects = new List<Collider>();
    public float radius = 1f;

    void Start()
    {
        radius = this.gameObject.GetComponent<SphereCollider>().radius;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {   //if it's an object with a rigidbody (moveable),
            //add to list of objects in collider...
            if (other.GetComponent<Rigidbody>() != null)
            {
                objects.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" )
        {   //if the objects leave the collider, remove from list.
            if (other.GetComponent<Rigidbody>() != null)
            {
                objects.Remove(other);
            }
        }
    }
}
