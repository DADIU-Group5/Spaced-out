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

TODO:

Make matching the ragdolled and animated root rotation and position more elegant. Now the transition works only if the ragdoll has stopped, as
the blending code will first wait for mecanim to start playing the get up animation to get the animated hip position and rotation. 
Unfortunately Mecanim doesn't (presently) allow one to force an immediate transition in the same frame. 
Perhaps there could be an editor script that precomputes the needed information.

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

    //The current state
    private RagdollState state = RagdollState.animated;
    //How long do we blend when transitioning from ragdolled to animated
    public float ragdollToMecanimBlendTime = 0.5f;
    private float mecanimToGetUpTransitionTime = 0.05f;
    //A helper variable to store the time when we transitioned from ragdolled to blendToAnim state
    private float blendStartTime = -100;
    //Additional vectores for storing the pose the ragdoll ended up in.
    private Vector3 ragdolledHipPosition, ragdolledHeadPosition, ragdolledFeetPosition;
    //Declare a list of body parts, initialized in Start()
    private List<BodyPart> bodyParts = new List<BodyPart>();
    //Declare an Animator member variable, initialized in Start to point to this gameobject's Animator component.
    private Animator animator;


    public void EnableRagdoll()
    {
        SetKinematic(false);
        animator.enabled = false;
        state = RagdollState.ragdolled;
    }

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
        ragdolledFeetPosition = 0.5f * (GetBonePosition(HumanBodyBones.LeftToes) + GetBonePosition(HumanBodyBones.RightToes));
        ragdolledHeadPosition = GetBonePosition(HumanBodyBones.Head);
        ragdolledHipPosition = GetBonePosition(HumanBodyBones.Hips);

        //Initiate the get up animation
        if (GetRagdollForward().y > 0)
        {
            animator.SetBool("GetUpFromBack", true);
        }
        else
        {
            animator.SetBool("GetUpFromBelly", true);
        }
    }

    private Vector3 GetRagdollForward()
    {
        return animator.GetBoneTransform(HumanBodyBones.Hips).forward;
    }

    void SetKinematic(bool newValue)
    {
        //For each of the components in the array, treat the component as a Rigidbody and set its isKinematic property
        foreach (Rigidbody body in GetComponentsInChildren<Rigidbody>())
        {
            body.isKinematic = newValue;
        }
    }

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

    private Vector3 GetBonePosition(HumanBodyBones bone) {
        return animator.GetBoneTransform(bone).position;
    }

    void LateUpdate()
    {
        //Clear the get up animation controls so that we don't end up repeating the animations indefinitely
        animator.SetBool("GetUpFromBelly", false);
        animator.SetBool("GetUpFromBack", false);

        //Blending from ragdoll back to animated
        if (state == RagdollState.blendToAnim)
        {
            if (Time.time <= blendStartTime + mecanimToGetUpTransitionTime)
            {
                //If we are waiting for Mecanim to start playing the get up animations, update the root of the mecanim
                //character to the best match with the ragdoll
                Vector3 animatedToRagdolled = ragdolledHipPosition - GetBonePosition(HumanBodyBones.Hips);
                Vector3 newRootPosition = transform.position + animatedToRagdolled;

                //Now cast a ray from the computed position downwards and find the highest hit that does not belong to the character 
                RaycastHit[] hits = Physics.RaycastAll(new Ray(newRootPosition, Vector3.down));
                //newRootPosition.y = 0;
                foreach (RaycastHit hit in hits)
                {
                    if (!hit.transform.IsChildOf(transform))
                    {
                        newRootPosition.y = Mathf.Max(newRootPosition.y, hit.point.y);
                    }
                }
                transform.position = newRootPosition;

                //Get body orientation in ground plane for both the ragdolled pose and the animated get up pose
                Vector3 ragdolledDirection = ragdolledHeadPosition - ragdolledFeetPosition;
                ragdolledDirection.y = 0;

                Vector3 meanFeetPosition = 0.5f * (GetBonePosition(HumanBodyBones.LeftFoot) + GetBonePosition(HumanBodyBones.RightFoot));
                Vector3 animatedDirection = GetBonePosition(HumanBodyBones.Head) - meanFeetPosition;
                animatedDirection.y = 0;

                //Try to match the rotations. Note that we can only rotate around Y axis, as the animated characted must stay upright,
                //hence setting the y components of the vectors to zero. 
                transform.rotation *= Quaternion.FromToRotation(animatedDirection.normalized, ragdolledDirection.normalized);
            }
            //compute the ragdoll blend amount in the range 0...1
            float ragdollBlendAmount = 1.0f - (Time.time - blendStartTime - mecanimToGetUpTransitionTime) / ragdollToMecanimBlendTime;
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
                return;
            }
        }
    }

}
