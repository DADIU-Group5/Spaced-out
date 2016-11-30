using UnityEngine;
using System.Collections;

public class SpaceZoom : MonoBehaviour {


    public float zoomSpeed = 5.0f;
    private bool zoomingIn = false;
    private bool zoomingOut = false;
    private Vector3 orgPosition;
    private Vector3 keyPosition;

    [Header("Cameras")]
    public GameObject zoomCamera;
    public GameObject key;
    public GameObject startPos;

    void Update()
    {
        if (zoomingIn)
        {
            float step = zoomSpeed * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, keyPosition) < 2f)
            {
                zoomingIn = false;
                StartCoroutine(waitAfterZooming());
            }
        }

        if (zoomingOut)
        {
            float step = zoomSpeed * 2 * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, orgPosition, step);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, orgPosition) < 1f)
            {
                zoomingOut = false;
                zoomCamera.SetActive(false);
                ToggleUI();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            zoomCamera.SetActive(true);
            zoomCamera.transform.position = startPos.transform.position;
            orgPosition = zoomCamera.transform.position;
            keyPosition = key.transform.position;
            zoomingIn = true;
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        var statusEvent = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, statusEvent);
    }

    IEnumerator waitAfterZooming()
    {
        yield return new WaitForSeconds(0.5f);
        zoomingOut = true;
    }

    // Use this for initialization
    void Start () {
	
	}
	
}
