using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {

    public Rigidbody ragdollRoot;
    private Animator animator;
    private Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        joints = GetComponentsInChildren<CharacterJoint>();

        // disable gravity on model
        foreach (Rigidbody body in rigidbodies)
        {
            body.useGravity = false;
        }
        // projection limits unwanted stretches in model
        foreach (CharacterJoint joint in joints)
        {
            joint.enableProjection = true;
        }

        Blend();
        //DisableRagdoll();
        //EnableRagdoll();
    }

    public void EnableRagdoll()
    {
        // enable ragdoll effect
        foreach (Rigidbody body in rigidbodies)
        {
            body.isKinematic = false;
        }

        // disable animations
        animator.enabled = false;
    }

    public void DisableRagdoll()
    {
        // disable ragdoll effect
        foreach (Rigidbody joint in rigidbodies)
        {
            joint.isKinematic = true;
        }
        ragdollRoot.isKinematic = false;

        // enable animations
        animator.enabled = true;
    }

    public void Blend()
    {
        EnableRagdoll();
        animator.enabled = true;
    }
}
