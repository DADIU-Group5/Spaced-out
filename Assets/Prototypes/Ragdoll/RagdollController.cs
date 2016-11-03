using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {

    public Rigidbody[] ragdollRig;
    public CharacterJoint[] ragdollCharacterJoint;
    public Rigidbody ragdollRoot;

    // Use this for initialization
    void Start()
    {
        //find ragdoll joints in ragdollrigged gameobject, acess their rigidbodies
        ragdollRig = this.GetComponentsInChildren<Rigidbody>();
        ragdollCharacterJoint = this.GetComponentsInChildren<CharacterJoint>();
        foreach (Rigidbody joint in ragdollRig)
        {
            //isKinematic
            joint.isKinematic = true;
        }
        ragdollRoot.isKinematic = false;
        ragdollRoot.useGravity = false;

        foreach (CharacterJoint charJoint in ragdollCharacterJoint)
        {
            charJoint.enableProjection = true;
        }
    }

    public void RagdollStart()
    {
        //start ragdolling
        foreach (Rigidbody joint in ragdollRig)
        {
            //isKinematic
            joint.isKinematic = false;
        }
        ragdollRoot.isKinematic = false;

        //disable animator here
    }

    public void RagdollStop()
    {
        //stop ragdolling
        foreach (Rigidbody joint in ragdollRig)
        {
            //isKinematic
            joint.isKinematic = true;
        }
        ragdollRoot.isKinematic = false;

        //enable animator here
    }
}
