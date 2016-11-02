using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour {

    public Rigidbody[] ragdollRig;
    public Rigidbody dave;

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
        //dave.isKinematic = false;
        //dave.useGravity = false;
    }

    public void RagdollStart()
    {
        //start ragdolling
        foreach (Rigidbody joint in ragdollRig)
        {
            //isKinematic
            joint.isKinematic = false;
        }
        //dave.isKinematic = false;
        //dave.useGravity = false;
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
        //dave.isKinematic = false;
        //dave.useGravity = false;
        //enable animator here
    }
}
