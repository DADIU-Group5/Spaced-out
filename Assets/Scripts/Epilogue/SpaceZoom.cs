using UnityEngine;
using System.Collections;

public class SpaceZoom : MonoBehaviour {

    [Tooltip("Zoomspeed - it's doubled on the way 'back'.")]
    public float zoomSpeed = 5.0f;
    private bool zoomingIn = false;
    private bool zoomingOut = false;
    private Vector3 orgPosition;
    public Vector3 keyPosition;
    private Quaternion orgRotation;
    private bool zoomedInAlready = false;

    [Header("Camera:")]
    public GameObject zoomCamera;
    [Header("Key and camera startposition:")]
    public GameObject key;
    public GameObject startPos;

    void Update()
    {
        if (zoomingIn)
        {
            float step = zoomSpeed * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

            if (zoomCamera.transform.rotation.y < orgRotation.y + 10)
                zoomCamera.transform.RotateAround(zoomCamera.transform.position, Vector3.forward + Vector3.right, 20 * Time.deltaTime);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, keyPosition) < 2f)
            {
                zoomingIn = false;
                StartCoroutine(waitAfterZooming());
            }
        }

        if (zoomingOut)
        {
            float step = zoomSpeed * 5 * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, orgPosition, step);

            if (zoomCamera.transform.rotation.y < orgRotation.y + 10)
                zoomCamera.transform.RotateAround(zoomCamera.transform.position, Vector3.back + Vector3.left, 20 * Time.deltaTime * 5);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, orgPosition) < 1f)
            {
                zoomingOut = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!zoomedInAlready)
            {
                zoomCamera.SetActive(true);
                zoomCamera.transform.position = startPos.transform.position;
                orgPosition = zoomCamera.transform.position;
                keyPosition = key.transform.position;
                zoomingIn = true;
                zoomedInAlready = true;
            }
        }
    }

    IEnumerator waitAfterZooming()
    {
        yield return new WaitForSeconds(0.5f);
        zoomingOut = true;
    }
	
}
