using UnityEngine;
using System.Collections.Generic;

public class SlipperyHazard : MonoBehaviour {

    //should be in direction of blue arrow
    [HideInInspector]
    public GameObject player;

    [Tooltip("Maximum speed in the sticky zone.")]
    public Vector3 maximumSpeed = new Vector3(3f, 3f, 3f);

    [Tooltip("How much the speed increases at each FixedUpdate")]
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
            if (Mathf.Abs( Vector3.Distance(rgb.velocity, maximumSpeed)) < 0.05f)
            {
                if (rgb)
                //foreach object in sticky influence, increase velocity. (Force instead?)
                rgb.velocity = rgb.velocity * (1f+speedIncrease);
            }
        }
    }

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
