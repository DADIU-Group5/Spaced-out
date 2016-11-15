using UnityEngine;
using System.Collections;

public class RagdollPusher : MonoBehaviour
{
    [Range(0.1f, 5)]
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
                if (hit.collider.tag == "Player")
                {
                    var dir = -(hit.point - hit.collider.transform.position).normalized;
                    hit.collider.GetComponent<Rigidbody>().AddForce(force * dir, ForceMode.VelocityChange);

                    AnimationBlender helper = GetComponent<AnimationBlender>();
                    helper.EnableRagdoll();
                }
            }
        }

        //Pressing space makes the character get up, assuming that the character root has
        //a RagdollHelper script
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationBlender helper = GetComponent<AnimationBlender>();
            helper.DisableRagdoll();
        }
    }
}
