using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fan : MonoBehaviour {

    float appliedForce;

    [Tooltip("Force of the wind.")]
    public float fanForce = 20f;

    [HideInInspector]
    public float distance = 20f;

    [Header("Start and End point of windzone:")]
    [Tooltip("Add/place a transform object at the START of the fan's influence")]
    public Transform startPos;
    [Tooltip("Add/place a transform object at the END of the fan's influence")]
    public Transform endPos;

    [HideInInspector]
    public Vector3 windDirection;

    [HideInInspector]
    public GameplayElement itemState;

    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    // Use this for initialization
    void Start () {
        distance =  Vector3.Distance(startPos.position, endPos.position);
        windDirection = Vector3.Normalize(endPos.position - startPos.position);
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //if the fan is off, stop influencing stuff.
        if (!itemState.On)
        {
            objects.Clear();
        }
        for (int i = 0; i < objects.Count; i++)
        {
            //foreach object in fan's influence, push in winddirection.
            Rigidbody rgb = objects[i].GetComponent<Rigidbody>();
            appliedForce = fanForce / (1f + distance * distance) * 1;
            rgb.AddForce(windDirection * appliedForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (itemState.On)
        {
            if (other.transform.tag == "Player" || other.transform.tag == "object" &&
                other.transform.GetComponent<GameplayElement>().movement == Movement.floatingItem)
            {   //if it's an object with a rigidbody (moveable),
                //add to list of objects in collider...
                objects.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object" &&
            other.transform.GetComponent<GameplayElement>().movement == Movement.floatingItem)
        {   //if the objects leave the collider, remove from list.
            if (objects.Contains(other))
            {
                objects.Remove(other);
            }
        }

    }
}
