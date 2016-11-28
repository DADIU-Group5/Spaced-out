using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
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
    
    // For ensuring that the player at some point starts slowing
    [Tooltip("Threshold for when velocity is reduced faster.")]
    public float slowDownThreshold = 2;

    private bool canSlowDown;
    private Rigidbody body;
    private PlayerController playerController;
    private RagdollAnimationBlender animationBlender;
    private bool ragdolling;
    private bool firedZoomOut = false;

    void Update()
    {
        if (body.velocity.magnitude < slowDownThreshold && firedZoomOut)
        {
            var evt = new ObserverEvent(EventName.CameraZoomIn);
            Subject.instance.Notify(gameObject, evt);
            firedZoomOut = false;
        }

        if (body.velocity.magnitude > slowDownThreshold && !firedZoomOut)
        {
            var evt = new ObserverEvent(EventName.CameraZoomOut);
            Subject.instance.Notify(gameObject, evt);
            firedZoomOut = true;
        }
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        animationBlender = GetComponentInChildren<RagdollAnimationBlender>();
    }

    void LateUpdate()
    {
        // do nothing if player is not moving
        if (body.velocity == Vector3.zero)
        {
            return;
        }

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

    private void OnCollisionEnter(Collision other)
    {
        // enable ragdoll
        if (body.velocity.magnitude > ragdollThreshold)
        {
            animationBlender.EnableRagdoll();
            ragdolling = true;
        }

        var evt = new ObserverEvent(EventName.Collision);
        evt.payload.Add(PayloadConstants.COLLISION_STATIC, other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"));
        evt.payload.Add(PayloadConstants.VELOCITY, body.velocity.magnitude);
        evt.payload.Add(PayloadConstants.POSITION, other.contacts[0].point);
        Subject.instance.Notify(gameObject, evt);
    }
}
