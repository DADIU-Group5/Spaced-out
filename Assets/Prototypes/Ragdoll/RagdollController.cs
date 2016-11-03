using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {

    public Rigidbody[] ragdollRig;
    public Rigidbody ragdollRoot;

    // Use this for initialization
    void Start()
    {
        //find ragdoll joints in ragdollrigged gameobject, acess their rigidbodies
        ragdollRig = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody joint in ragdollRig)
        {
            //isKinematic
            joint.isKinematic = true;
        }
        ragdollRoot.isKinematic = false;
        ragdollRoot.useGravity = false;
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
        ragdollRoot.useGravity = false;

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
        ragdollRoot.useGravity = false;

        //enable animator here
    }
}
