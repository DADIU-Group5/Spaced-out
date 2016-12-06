using UnityEngine;
using System.Collections;

public class DaveMenuBackground : MonoBehaviour {

    [Tooltip("Controls the point of which dave will go through")]
    public Transform daveHit;
    [Tooltip("Dave's launch speed")]
    public float speed;
    [Tooltip("Time Dave will fly until a new launch will happen")]
    public float duration;
    [Tooltip("Probability of ragdolling")]
    [Range(0, 1)]
    public float ragdollProbability;

    private Rigidbody body;
    private RagdollAnimationBlender blender;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        blender = GetComponentInChildren<RagdollAnimationBlender>();
        InvokeRepeating("LaunchDave", 0f, duration);
	}
	
    // launches dave in a random direction in the background
	private void LaunchDave()
    {
        // calculate distance and direction
        float distance = (speed * duration) / 2f;
        Vector3 direction = Random.insideUnitCircle.normalized;

        if (Random.value < ragdollProbability)
        {
            // ragdoll dave
            blender.EnableRagdoll();
            body.angularVelocity = Random.insideUnitCircle.normalized * 5f;
        }
        else
        {
            // animate dave
            blender.DisableRagdoll();
            body.angularVelocity = Vector3.zero;
        }

        // set daves position and speed
        transform.position = daveHit.position - direction * distance;
        transform.forward = direction;
        body.velocity = direction * speed;
    }
}
