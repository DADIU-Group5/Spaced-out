using UnityEngine;
using System.Collections;

public class Astroid : MonoBehaviour {

    public Vector3 rotation;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddTorque(rotation, ForceMode.VelocityChange);
	}
}
