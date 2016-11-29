using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Declare a class that will hold useful information for each body part
public class BodyPart
{
    public Transform transform;
    public Vector3 storedPosition;
    public Quaternion storedRotation;
}

[RequireComponent(typeof(Animator))]
public class RagdollAnimationBlender : MonoBehaviour
{
    private enum RagdollState
    {
        animating, ragdolling, blending
    }

    [Tooltip("Curve to determine how to blend from ragdoll to animations. 0 = fully ragdoll, 1 = fully animated")]
    public AnimationCurve blendCurve = AnimationCurve.Linear(0, 0, 2, 1);
    private RagdollState state = RagdollState.animating;
    private List<BodyPart> bodyParts = new List<BodyPart>();
    private Animator animator;
    private float time;

    // Initialization, first frame of game
    void Awake()
    {
        animator = GetComponent<Animator>();
        SetupRagdoll();
    }

    // setup the ragdoll with correct settings
    private void SetupRagdoll()
    {
        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            BodyPart bodyPart = new BodyPart();
            bodyPart.transform = t;
            bodyParts.Add(bodyPart);
        }

        // disable physics and gravity
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.useGravity = false;
            body.isKinematic = true;
        }

        // projection helps making the ragdoll more stable
        foreach (CharacterJoint joint in GetComponentsInChildren<CharacterJoint>())
        {
            joint.enableProjection = true;
        }
    }

    public void EnableRagdoll()
    {
        SetKinematic(false);
        animator.enabled = false;
        state = RagdollState.ragdolling;
    }

    public void DisableRagdoll()
    {
        SetKinematic(true);
        animator.enabled = true;
        state = RagdollState.blending;
        time = 0;

        //Store the ragdolled position for blending
        foreach (BodyPart b in bodyParts)
        {
            b.storedRotation = b.transform.localRotation;
            b.storedPosition = b.transform.localPosition;
        }
    }

    // set isKinematic for all parts of the ragdoll
    void SetKinematic(bool newValue)
    {
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = newValue;
        }
    }

    // blends from ragdoll to animations. t = 0 = ragdoll, t = 1 = animations
    private void Blend(float t)
    {
        foreach (BodyPart b in bodyParts)
        {
            if (b.transform != transform)
            {
                b.transform.localPosition = Vector3.Lerp(b.storedPosition, b.transform.localPosition, t);
                b.transform.localRotation = Quaternion.Slerp(b.storedRotation, b.transform.localRotation, t);
            }
        }
    }

    void LateUpdate()
    {
        FakeInput();

        if (state == RagdollState.blending)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(blendCurve.Evaluate(time));

            if (t == 1)
                state = RagdollState.animating; // finished the blending
            else
                Blend(t);
        }
    }

    private void FakeInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            EnableRagdoll();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            DisableRagdoll();
        }
    }
}
