﻿using UnityEngine;
using System.Collections;

public class ExplosiveBarrelHazard : MonoBehaviour {

    //determined by playerspeed, etc.
    [HideInInspector]
    public float pushForce = 50f;

    [Header("Power of the explosion:")]
    public float explosionPower = 50f;

    [Header("In the damaging radius:")]
    public float explosionRadius = 10f;
    [Header("In the pushing radius:")]
    public float pushRadius = 5f;

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
        StartCoroutine(Flasher());

        //wait...
        yield return new WaitForSeconds(timeToExplode);

        //Explode, and destroy object/start animation:

        //find all objects in radius of child's spherecollider...
        Collider[] explosionObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < explosionObjects.Length; i++)
        {
            Rigidbody rgb = explosionObjects[i].GetComponent<Rigidbody>();
            if (rgb == null)
                continue;
            if (explosionObjects[i].tag == "Player")
            {
                explosionObjects[i].GetComponent<PlayerController>().dead = true;
            }
        }

        Collider[] pushObjects = Physics.OverlapSphere(transform.position, pushRadius);

        for (int i = 0; i < pushObjects.Length; i++)
        {
            Rigidbody rgb = pushObjects[i].GetComponent<Rigidbody>();
            if (rgb == null)
                continue;
            //and Boom!

            // transform.position should be pushDirection - check results.
            rgb.AddExplosionForce(explosionPower, transform.position, explosionRadius);
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
        if (other.transform.tag == "Player" || 
            other.transform.tag == "object" && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            pushForce = other.rigidbody.velocity.magnitude;
            pushDirection = other.contacts[0].point - transform.position;
            pushDirection = -pushDirection.normalized;

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
