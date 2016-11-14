﻿using UnityEngine;
using System.Collections;

public class RagdollPusher : MonoBehaviour
{
    [Range(0.5f, 5)]
    public float force = 2f;



    //Declare a member variables for distributing the impacts over several frames
    float impactEndTime = 0;
    Rigidbody impactTarget = null;
    Vector3 impact;

    // Update is called once per frame
    void Update()
    {
        //if left mouse button clicked
        if (Input.GetMouseButtonDown(0))
        {
            //Get a ray going from the camera through the mouse cursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //check if the ray hits a physic collider
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //check if the raycast target has a rigid body (belongs to the ragdoll)
                if (hit.rigidbody != null)
                {
                    //find the RagdollHelper component and activate ragdolling
                    RagdollHelper helper = GetComponent<RagdollHelper>();
                    helper.EnableRagdoll();

                    //set the impact target to whatever the ray hit
                    impactTarget = hit.rigidbody;

                    //impact direction also according to the ray
                    impact = ray.direction * 2.0f;

                    //the impact will be reapplied for the next 250ms
                    //to make the connected objects follow even though the simulated body joints
                    //might stretch
                    impactEndTime = Time.time + 0.25f;
                }
            }
        }

        //Pressing space makes the character get up, assuming that the character root has
        //a RagdollHelper script
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RagdollHelper helper = GetComponent<RagdollHelper>();
            helper.DisableRagdoll();
        }

        //Check if we need to apply an impact
        if (Time.time < impactEndTime)
        {
            impactTarget.AddForce(impact, ForceMode.VelocityChange);
        }
    }
}