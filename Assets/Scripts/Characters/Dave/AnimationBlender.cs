using UnityEngine;
using System.Collections.Generic;



public class AnimationBlender : MonoBehaviour
{
    // a class that will hold useful information for each body part
    public class BodyPart
    {
        public Transform playerTransform;
        public Transform bodyPartTransform;
        private Vector3 storedPos;
        private Quaternion storedRot;


        public BodyPart(Transform player, Transform bodyPart)
        {
            playerTransform = player;
            bodyPartTransform = bodyPart;
        }

        // creates a snapshot of the position and rotation
        public void Snapshot()
        {
            storedPos = GetRelativePosition();
            storedRot = GetRelativeRotation();
        }

        // returns the position relative to player
        private Vector3 GetRelativePosition()
        {
            return bodyPartTransform.position - playerTransform.position;
        }

        // returns the rotation in euler angles relative to player
        private Quaternion GetRelativeRotation()
        {
            return bodyPartTransform.rotation * Quaternion.Inverse(playerTransform.rotation);
        }

        // Lerps the position relative to the player
        public Vector3 LerpPosition(float t)
        {
            return Vector3.Lerp(storedPos, GetRelativePosition(), t);
        }

        // Lerps the rotation relative to the player
        public Quaternion LerpRotation(float t)
        {
            return Quaternion.Lerp(storedRot, GetRelativeRotation(), t);
        }
    }

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
    public Animator animator;

    // Initialization, first frame of game
    void Start()
    {
        SetKinematic(true);

        
        //For each of the transforms, create a BodyPart instance and store the transform 
        foreach (Transform t in hips.GetComponentsInChildren<Transform>())
        {
            //if (t != hips.transform)
            bodyParts.Add(new BodyPart(transform, t));
        }
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
                SetKinematic(true);
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
        //SetKinematic(true);
        blendStartTime = Time.time;
        animator.enabled = true;
        state = RagdollState.blendToAnim;

        //Store the ragdolled position for blending
        foreach (BodyPart b in bodyParts)
        {
            b.Snapshot();
        }

        //Initiate the get up animation
        animator.SetBool("Dance", true);
    }

    // blend between animation and ragdoll
    // t = 0 is full ragdoll, t = 1 is full animation
    private void BlendToAnimation(float t)
    {
        // interpolate position and rotation of all body parts
        foreach (BodyPart b in bodyParts)
        {
            // don't interpolate hips position
            if (b.bodyPartTransform != hips.transform)
                b.bodyPartTransform.position = b.LerpPosition(t) + transform.position;

            b.bodyPartTransform.rotation = b.LerpRotation(t) * transform.rotation;
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
