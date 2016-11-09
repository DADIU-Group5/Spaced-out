﻿using UnityEngine;
using System.Collections.Generic;

public class StickyHazard : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

    [Tooltip("Minimum speed in the sticky zone.")]
    public Vector3 minimumSpeed = new Vector3(.1f,.1f,.1f);

    [Tooltip("How much the speed decreases at each FixedUpdate")]
    public float speedDecrease = 0.1f;

    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    [HideInInspector]
    public GameplayElement itemState;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Rigidbody rgb = objects[i].GetComponent<Rigidbody>();
            if (Mathf.Abs(Vector3.Distance(rgb.velocity, minimumSpeed)) > 0.01f)
            {
                //foreach object in sticky influence, lower velocity. (Force instead?)
                rgb.velocity = rgb.velocity * (1f-speedDecrease);
            }
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
