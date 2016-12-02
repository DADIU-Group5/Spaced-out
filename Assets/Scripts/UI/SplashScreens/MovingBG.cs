using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MovingBG : MonoBehaviour {

    public float moveSpeed = 0.5f;
    public float tileSizeZ;
    private Vector2 offset;

	// Update is called once per frame
	void Update () {
        /*float newPosition = Mathf.Repeat(Time.time * moveSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPosition;*/

        offset = new Vector2(0, - Time.time * moveSpeed);

        GetComponent<Renderer>().material.mainTextureOffset = offset;
        


        //FadeIn();
    }

    void FadeIn()
    {
        //Logic for fading in screen. Maybe place Dadiu screen behind this background,
        //so the bg graphics overrides it naturally.
    }
}
