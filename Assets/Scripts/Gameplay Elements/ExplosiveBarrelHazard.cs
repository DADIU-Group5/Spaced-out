using UnityEngine;
using System.Collections.Generic;

public class ExplosiveBarrelHazard : MonoBehaviour {

    //determined by playerspeed, etc.
    [HideInInspector]
    public float pushForce = 50f;

    public float explosionPower = 50f;

    public ExplosionTargets explosionRadius;

    //direction of the push, based on collision
    [HideInInspector]
    public Vector3 pushDirection;

    [Tooltip("The force from the player at which the barrel explodes.")]
    public float explodeForce = 0.5f;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || 
            other.transform.tag == "object" && other.gameObject.GetComponent<Rigidbody>() != null)
        {
            //objects.Add(other);
            pushForce = other.rigidbody.velocity.magnitude;
            if (pushForce >= explodeForce)
            {
                Debug.Log("         Exploding!!         ");
                
                if (explosionRadius.objects.Count > 0)
                {
                    for (int i = 0; i < explosionRadius.objects.Count; i++)
                    {
                        Debug.Log("adding force to object...");
                        Rigidbody rgb = explosionRadius.objects[i].GetComponent<Rigidbody>();
                        //foreach object in sticky influence, lower velocity. (Force instead?)
                        rgb.AddExplosionForce(explosionPower, transform.position, explosionRadius.radius);
                    }
                }
            }
            else
            {
                pushDirection = other.contacts[0].point - transform.position;
                pushDirection = -pushDirection.normalized;
                GetComponent<Rigidbody>().AddForce(pushDirection * pushForce);
            }
        }
    }

    /*//should it return when not touched?
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {   
            
        }
    }*/
}
