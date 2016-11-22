using UnityEngine;
using System.Collections;

public class TutorialCamera : MonoBehaviour {

    public AnimationCurve curve;
    public float animationTime;
    public TutorialBehaviour tutorial;
    private Vector3 startPos;
    private float time = -1;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (time != -1)
        {
            time += Time.deltaTime;
            transform.position = startPos + new Vector3(curve.Evaluate(time / animationTime), 0, 0);
            if (time >= animationTime)
            {
                time = -1;
                tutorial.EnableControl();
            }
        }
    }

	public void Animate()
    {
        time = 0;
    }
}
