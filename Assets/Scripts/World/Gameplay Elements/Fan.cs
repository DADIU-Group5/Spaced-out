﻿using UnityEngine;
using System.Collections.Generic;
public class Fan : MonoBehaviour
{
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

    [Header("The Fan blades object:")]
    public GameObject fanBlades;
    private Vector3 rotationDirection = Vector3.right;

    [HideInInspector]
    public Vector3 windDirection;

    [HideInInspector]
    public GameplayElement itemState;

    // Internal list that tracks objects that enter this object's "zone"
    private List<Collider> objects = new List<Collider>();

    void Update()
    {
        if (itemState.On)
            fanBlades.transform.RotateAround(fanBlades.transform.position, rotationDirection, 20 * Time.deltaTime * 20);
    }

    // Use this for initialization
    void Awake()
    {
        distance = Vector3.Distance(startPos.position, endPos.position);
        CapsuleCollider coll = GetComponent<CapsuleCollider>();
        coll.height = endPos.transform.localPosition.x;
        coll.center = new Vector3(coll.height / 2, 0, 0);
        windDirection = Vector3.Normalize(endPos.position - startPos.position);
        if(fanForce < 0)
        {
            rotationDirection = -transform.right;
        }
        else
        {
            rotationDirection = transform.right;
        }


        itemState = gameObject.GetComponent<GameplayElement>();
    }

    //for every frame, for every collider touching trigger.
    void OnTriggerStay(Collider other)
    {
        if (itemState == null)
        {
            return;
        }
        if (itemState.On)
        {
            if (other.transform.tag == "Player" || other.transform.tag == "Floating Object" /*&&
                other.transform.GetComponent<GameplayElement>().movement == Movement.floatingItem*/)
            {

                Rigidbody rgb = other.GetComponent<Rigidbody>();
                if (other.tag == "Player")
                {
                    appliedForce = fanForce * 60 / (1f + distance * distance) * 1;
                }
                else
                {
                    appliedForce = fanForce / (1f + distance * distance) * 1;
                }
                rgb.AddForce(windDirection * appliedForce);

            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (itemState.On && other.transform.tag == "Player")
        {
            var evt = new ObserverEvent(EventName.PlayerVentilated);
            Subject.instance.Notify(gameObject, evt);
        }
    }
}
