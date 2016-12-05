using UnityEngine;
using System.Collections;
using System;

public class TutorialTrigger : MonoBehaviour {

    public Action callback;
    public GameObject door;
    public GameObject launchAt;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            door.SetActive(true);
            callback.Invoke();
            //collider.transform.position = gameObject.transform.position;
            //collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //collider.GetComponent<PlayerController>().Aim(launchAt.transform.position);
            Destroy(gameObject);
        }
    }
}