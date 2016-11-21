using UnityEngine;
using System.Collections;

public class RandomInitialForce : MonoBehaviour {

    public RagdollAnimationBlender anim;

    void Start () {

        anim.EnableRagdoll();
        GetComponent<Rigidbody>().AddForce(new Vector3(8000f,0f,0f));
	}
}
