using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fan : MonoBehaviour {

    float appliedForce;
    public float fanForce = 20f;
    public float distance = 20f;
    public Transform startPos;
    public Transform endPos;
    public Vector3 windDirection;
    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    // Use this for initialization
    void Start () {
        distance =  Vector3.Distance(startPos.position, endPos.position);
        windDirection = Vector3.Normalize(endPos.position - startPos.position); 
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < objects.Count; i++)
        {
            Rigidbody rgb = objects[i].GetComponent<Rigidbody>();
            appliedForce = fanForce / (1f + distance * distance) * 1;
            rgb.AddForce(windDirection * appliedForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" && 
            other.transform.GetComponent<Item>().movement == Movement.floatingItem)
        {
            objects.Add(other);
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "player" || other.transform.tag == "object" &&
            other.transform.GetComponent<Item>().movement == Movement.floatingItem)
        {
            objects.Remove(other);
        }

    }
}
