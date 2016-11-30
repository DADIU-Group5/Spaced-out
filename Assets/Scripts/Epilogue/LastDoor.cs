using UnityEngine;
using System.Collections;

public class LastDoor : MonoBehaviour
{

    [Header("Cameras")]
    public GameObject playerCameraPod;
    public GameObject introCamera;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
            ToggleUI();
            playerCameraPod.SetActive(false);
            introCamera.gameObject.SetActive(true);
        }
    }
}
