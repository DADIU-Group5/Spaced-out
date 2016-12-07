using UnityEngine;
using System.Collections;

public class CollectibleRotator : MonoBehaviour {

    public float rotationSpeed;
    public AnimationCurve vertical;

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotationSpeed);
        transform.Translate(Vector3.up * vertical.Evaluate(Time.time));
	}
}
