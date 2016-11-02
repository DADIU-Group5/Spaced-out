using UnityEngine;
using System.Collections.Generic;

public class SlipperyHazard : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

    [Tooltip("Minimum speed in the sticky zone.")]
    public Vector3 minimumSpeed = new Vector3(.1f, .1f, .1f);

    public float speedIncrease = 0.05f;

    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Rigidbody rgb = objects[i].GetComponent<Rigidbody>();
            if (Vector3.Distance(rgb.velocity, minimumSpeed) > 0.01f)
            {
                //foreach object in sticky influence, increase velocity. (Force instead?)
                rgb.velocity = rgb.velocity * (1f+speedIncrease);
                //Debug.Log("slippery...");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" &&
            other.transform.GetComponent<Item>().movement == Movement.floatingItem)
        {   //if it's an object with a rigidbody (moveable),
            //add to list of objects in collider...
            objects.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Object left zone");
        if (other.transform.tag == "Player" || other.transform.tag == "object" &&
            other.transform.GetComponent<Item>().movement == Movement.floatingItem)
        {   //if the objects leave the collider, remove from list.
            Debug.Log("Object removed");
            objects.Remove(other);
        }
    }
}
