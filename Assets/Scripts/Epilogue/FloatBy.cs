using UnityEngine;
using System.Collections;

public class FloatBy : MonoBehaviour {

    [Header("Cameras")]
    public GameObject doorCameraPod;
    public GameObject floatCamera;
    public GameObject WinMenu;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           // ToggleUI();
            doorCameraPod.SetActive(false);
            floatCamera.gameObject.SetActive(true);

            for (int i = 0; i < WinMenu.transform.childCount; i++)
            {
                WinMenu.transform.GetChild(i).gameObject.SetActive(true);
            }

        }
    }
}
