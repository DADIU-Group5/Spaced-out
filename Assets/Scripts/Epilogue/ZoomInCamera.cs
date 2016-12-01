using UnityEngine;
using System.Collections;

public class ZoomInCamera : MonoBehaviour {

    [Header("Cameras")]
    public GameObject mainCameraPod;
    public GameObject zoomCamera;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ToggleUI()
    {
        var statusEvent = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, statusEvent);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //ToggleUI();
            mainCameraPod.SetActive(false);
            zoomCamera.gameObject.SetActive(true);
        }
    }
}
