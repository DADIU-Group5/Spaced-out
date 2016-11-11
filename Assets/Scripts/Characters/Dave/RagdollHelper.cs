using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
A helper component that enables blending from Mecanim animation to ragdolling and back. 

To use, do the following:

Add "GetUpFromBelly" and "GetUpFromBack" bool inputs to the Animator controller
and corresponding transitions from any state to the get up animations. When the ragdoll mode
is turned on, Mecanim stops where it was and it needs to transition to the get up state immediately
when it is resumed. Therefore, make sure that the blend times of the transitions to the get up animations are set to zero.

*/

//Declare a class that will hold useful information for each body part
public class BodyPart
{
    public Transform transform;
    public Vector3 storedPosition;
    public Quaternion storedRotation;
}

public class RagdollHelper : MonoBehaviour
{
    enum RagdollState
    {
        animated,    //Mecanim is fully in control
        ragdolled,   //Mecanim turned off, physics controls the ragdoll
        blendToAnim  //Mecanim in control, but LateUpdate() is used to partially blend in the last ragdolled pose
    }

    //How long do we blend when transitioning from ragdolled to animated
    [Range(0.1f, 4f)]
    public float ragdollToMecanimBlendTime = 1f;

    private RagdollState state = RagdollState.animated;
    private float blendStartTime = -100;
    //Additional vectores for storing the pose the ragdoll ended up in.
    private Vector3 ragdolledHipPosition, ragdolledHeadPosition, ragdolledFeetPosition;
    private List<BodyPart> bodyParts = new List<BodyPart>();
    private Animator animator;

    // Initialization, first frame of game
    void Start()
    {
        SetKinematic(true);

        //Find all the transforms in the character, assuming that this script is attached to the root
        Component[] components = GetComponentsInChildren(typeof(Transform));

        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Transform t in GetComponentsInChildren<Transform>())
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
            if (Time.time <= blendStartTime)
            {






                //If we are waiting for Mecanim to start playing the get up animations, update the root of the mecanim
                //character to the best match with the ragdoll
                //Vector3 animatedToRagdolled = ragdolledHipPosition - GetBonePosition(HumanBodyBones.Hips);
                //Vector3 newRootPosition = transform.position + animatedToRagdolled;

                //Now cast a ray from the computed position downwards and find the highest hit that does not belong to the character 
                //RaycastHit[] hits = Physics.RaycastAll(new Ray(newRootPosition, Vector3.down));
                ////newRootPosition.y = 0;
                //foreach (RaycastHit hit in hits)
                //{
                //    if (!hit.transform.IsChildOf(transform))
                //    {
                //        newRootPosition.y = Mathf.Max(newRootPosition.y, hit.point.y);
                //    }
                //}
                //transform.position = newRootPosition;

                //Get body orientation in ground plane for both the ragdolled pose and the animated get up pose
                //Vector3 ragdolledDirection = ragdolledHeadPosition - ragdolledFeetPosition;
                //ragdolledDirection.y = 0;

                //Vector3 meanFeetPosition = 0.5f * (GetBonePosition(HumanBodyBones.LeftFoot) + GetBonePosition(HumanBodyBones.RightFoot));
                //Vector3 animatedDirection = GetBonePosition(HumanBodyBones.Head) - meanFeetPosition;
                //animatedDirection.y = 0;

                //Try to match the rotations. Note that we can only rotate around Y axis, as the animated characted must stay upright,
                //hence setting the y components of the vectors to zero. 
                //transform.rotation *= Quaternion.FromToRotation(animatedDirection.normalized, ragdolledDirection.normalized);
            }




            //compute the ragdoll blend amount in the range 0...1
            float ragdollBlendAmount = 1.0f - (Time.time - blendStartTime) / ragdollToMecanimBlendTime;
            ragdollBlendAmount = Mathf.Clamp01(ragdollBlendAmount);

            //In LateUpdate(), Mecanim has already updated the body pose according to the animations. 
            //To enable smooth transitioning from a ragdoll to animation, we lerp the position of the hips 
            //and slerp all the rotations towards the ones stored when ending the ragdolling
            foreach (BodyPart b in bodyParts)
            {
                if (b.transform != transform)
                { //this if is to prevent us from modifying the root of the character, only the actual body parts
                  //position is only interpolated for the hips
                    if (b.transform == animator.GetBoneTransform(HumanBodyBones.Hips))
                        b.transform.position = Vector3.Lerp(b.transform.position, b.storedPosition, ragdollBlendAmount);
                    //rotation is interpolated for all body parts
                    b.transform.rotation = Quaternion.Slerp(b.transform.rotation, b.storedRotation, ragdollBlendAmount);
                }
            }

            //if the ragdoll blend amount has decreased to zero, move to animated state
            if (ragdollBlendAmount == 0)
            {
                state = RagdollState.animated;
                //Clear the get up animation controls so that we don't end up repeating the animations indefinitely
                animator.SetBool("Dance", false);
                return;
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

        //Remember some key positions
        //ragdolledFeetPosition = 0.5f * (GetBonePosition(HumanBodyBones.LeftToes) + GetBonePosition(HumanBodyBones.RightToes));
        //ragdolledHeadPosition = GetBonePosition(HumanBodyBones.Head);
        //ragdolledHipPosition = GetBonePosition(HumanBodyBones.Hips);

        //Initiate the get up animation
        animator.SetBool("Dance", true);
    }

    // blend between animation and ragdoll
    // t = 0 is full ragdoll, t = 1 is full animation
    private void BlendToAnimation(float t)
    {
        t = Mathf.Clamp01(t);

    }

    // get a forward direction of the ragdoll
    private Vector3 GetRagdollForward()
    {
        return animator.GetBoneTransform(HumanBodyBones.Hips).forward;
    }

    // sets the kinematic of all bones in the ragdoll
    private void SetKinematic(bool newValue)
    {
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = newValue;
        }
    }

    // get the position of a bone in the animation
    private Vector3 GetBonePosition(HumanBodyBones bone) {
        return animator.GetBoneTransform(bone).position;
    }
}
