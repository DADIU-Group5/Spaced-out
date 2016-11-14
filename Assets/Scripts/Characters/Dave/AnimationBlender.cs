using UnityEngine;
using System.Collections.Generic;

// a class that will hold useful information for each body part
public class BodyPart
{
    public Transform transform;
    public Vector3 storedPosition;
    public Quaternion storedRotation;
}

public class AnimationBlender : MonoBehaviour
{
    enum RagdollState
    {
        animated,    //Mecanim is fully in control
        ragdolled,   //Mecanim turned off, physics controls the ragdoll
        blendToAnim  //Mecanim in control, but LateUpdate() is used to partially blend in the last ragdolled pose
    }

    [Tooltip("Controls the animation blending. 0 = full ragdoll, 1 = full animation")]
    public AnimationCurve blendCurve = AnimationCurve.EaseInOut(0, 0, 2f, 1);
    [Tooltip("The root bone of the ragdoll")]
    public Rigidbody hips;

    private RagdollState state = RagdollState.animated;
    private float blendStartTime = -1;
    private List<BodyPart> bodyParts = new List<BodyPart>();
    private Animator animator;

    // Initialization, first frame of game
    void Start()
    {
        SetKinematic(true);

        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Transform t in hips.GetComponentsInChildren<Transform>())
        {
            BodyPart bodyPart = new BodyPart();
            bodyPart.transform = t;
            bodyParts.Add(bodyPart);
        }

        //Store the Animator component
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        //Blending from ragdoll back to animated
        if (state == RagdollState.blendToAnim)
        {
            // amount to blend animation
            float t = blendCurve.Evaluate(Time.time - blendStartTime);
            t = Mathf.Clamp01(t);

            
            // check if blend phase is finished
            if (t == 1)
            {
                state = RagdollState.animated;
                animator.SetBool("Dance", false);
            }
            else
            {
                BlendToAnimation(t);
            }
        }
    }

    // enables the ragdoll
    public void EnableRagdoll()
    {
        SetKinematic(false);
        animator.enabled = false;
        state = RagdollState.ragdolled;
    }

    // disables the ragdoll
    public void DisableRagdoll()
    {
        SetKinematic(true);
        blendStartTime = Time.time;
        animator.enabled = true;
        state = RagdollState.blendToAnim;

        //Store the ragdolled position for blending
        foreach (BodyPart b in bodyParts)
        {
            b.storedRotation = b.transform.rotation;
            b.storedPosition = b.transform.position;
        }

        //Initiate the get up animation
        animator.SetBool("Dance", true);
    }

    // blend between animation and ragdoll
    // t = 0 is full ragdoll, t = 1 is full animation
    private void BlendToAnimation(float t)
    {
        // interpolate position and rotation of all bod parts
        foreach (BodyPart b in bodyParts)
        {
            if (b.transform != transform)
            {
                // don't interpolate the position of the hip
                if (b.transform == animator.GetBoneTransform(HumanBodyBones.Hips))
                    b.transform.position = Vector3.Lerp(b.storedPosition, b.transform.position, t);

                //rotation is interpolated for all body parts
                b.transform.rotation = Quaternion.Slerp(b.storedRotation, b.transform.rotation, t);
            }
        }
    }

    // get a forward direction of the ragdoll
    private Vector3 GetRagdollForward()
    {
        return animator.GetBoneTransform(HumanBodyBones.Hips).forward;
    }

    // sets the kinematic of all bones in the ragdoll
    private void SetKinematic(bool newValue)
    {
        foreach (Rigidbody body in hips.GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = newValue;
        }
    }

    // get the position of a bone in the animation
    private Vector3 GetBonePosition(HumanBodyBones bone) {
        return animator.GetBoneTransform(bone).position;
    }
}
