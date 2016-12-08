using UnityEngine;
using System.Collections;

public class CollectibleRotator : MonoBehaviour {

    public float rotationSpeed, hoverScale = 5f;
    public AnimationCurve vertical;
    public Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
        //transform.Translate(Vector3.up * vertical.Evaluate(Time.time));
        transform.position = startPos + Vector3.up * vertical.Evaluate(Time.time) * hoverScale;
	}
}
