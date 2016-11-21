using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OxygenController))]
public class MovementBehaviour : MonoBehaviour
{
    // For ensuring that the player at some point starts slowing
    [Tooltip("Player will stop ragdolling when moving slower than this threshold")]
    public float ragdollThreshold = 4;
    [Tooltip("Player is slowed down when moving slower than this threshold")]
    public float slowThreshold = 2;
    [Tooltip("Player is stopped when moving slower than this threshold")]
    public float stopThreshold = 0.2f;
    [Tooltip("How fast will the player be slowed down")]
    [Range(0, 1)]
    public float slowFactor = 0.02f;

    private bool canSlowDown;
    private Rigidbody body;
    private OxygenController oxygen;
    private PlayerController playerController;
    private RagdollAnimationBlender animationBlender;
    private bool ragdolling;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        oxygen = GetComponent<OxygenController>();
        playerController = GetComponent<PlayerController>();
        animationBlender = GetComponentInChildren<RagdollAnimationBlender>();
    }

    void LateUpdate()
    {
        // do nothing if player is not moving
        if (body.velocity == Vector3.zero)
            return;

        // disable ragdoll
        if (ragdolling && body.velocity.magnitude < ragdollThreshold)
        {
            animationBlender.DisableRagdoll();
            ragdolling = false;
        }

        // slow down player
        if (body.velocity.magnitude < slowThreshold)
        {
            body.velocity = body.velocity * (1f - slowFactor);
        }

        // stop player
        if (body.velocity.magnitude < stopThreshold)
        {
            body.velocity = Vector3.zero;
            playerController.ReadyForLaunch();
        }
    }

    private void OnCollisionEnter()
    {
        // enable ragdoll
        print("Velocity: " + body.velocity.magnitude);
        if (body.velocity.magnitude > ragdollThreshold)
        {
            animationBlender.EnableRagdoll();
            ragdolling = true;
        }
    }

}
