using UnityEngine;
using System.Collections;
using System;

public class TutorialTrigger : MonoBehaviour {

    public Action callback;
    public GameObject door;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            door.SetActive(true);
            callback.Invoke();
            Destroy(gameObject);
        }
    }
}