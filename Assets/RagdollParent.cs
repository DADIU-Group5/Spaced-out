using UnityEngine;
using System.Collections;

public class RagdollParent : MonoBehaviour {

    public Transform hips;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = hips.position;
        transform.rotation = hips.rotation;

        hips.position = Vector3.zero;
        hips.rotation = new Quaternion();
	}
}
