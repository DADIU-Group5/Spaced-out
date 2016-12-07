using UnityEngine;
using System.Collections;

public class InitialForce : MonoBehaviour {

    public bool applyOnStart = true;
    public bool applyOnlyTorque;
    private float force = 3;
    private Rigidbody body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        if (applyOnStart)
        {
            if (applyOnlyTorque)
                ApplyTorque();
            else
                ApplyForce();
        }
	}

    public void ApplyTorque()
    {
        var min = 0.5f * Vector3.one;
        var torque = min + Random.insideUnitSphere * 0.5f;
        body.AddTorque(torque);
    }

    public void ApplyForce()
    {
        body.isKinematic = false;
        body.AddForce(Random.onUnitSphere * force, ForceMode.Force);
        ApplyTorque();
    }
}
