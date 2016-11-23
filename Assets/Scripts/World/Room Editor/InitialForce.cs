using UnityEngine;
using System.Collections;

public class InitialForce : MonoBehaviour {

    float force = 3;

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddForce(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force), ForceMode.Force);
	}
}
