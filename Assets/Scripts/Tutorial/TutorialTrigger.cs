using UnityEngine;
using System.Collections;
using System;

public class TutorialTrigger : MonoBehaviour {

    public Action callback;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            callback.Invoke();
            Destroy(gameObject);
        }
    }
}