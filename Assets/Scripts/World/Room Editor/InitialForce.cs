using UnityEngine;
using System.Collections;

public class InitialForce : MonoBehaviour {

    public bool applyOnStart = true; 
    private float force = 3;

	// Use this for initialization
	void Start () {
        if (applyOnStart)
            ApplyForce();
	}

    public void ApplyForce()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        float x = Random.Range(-force, force);
        float y = Random.Range(-force, force);
        float z = Random.Range(-force, force);
        GetComponent<Rigidbody>().AddForce(new Vector3(x, y, z), ForceMode.Force);
    }
}
