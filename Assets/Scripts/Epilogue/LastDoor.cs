using UnityEngine;
using System.Collections;

public class LastDoor : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject playerCameraPod;
    public GameObject introCamera;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCameraPod.SetActive(false);
            introCamera.gameObject.SetActive(true);
        }
    }
}
