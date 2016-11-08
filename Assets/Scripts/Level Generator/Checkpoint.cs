using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    bool wasActivated = false;

    void Start()
    {
        //Debug.Log("spawn location = " + (transform.position - (transform.right * 1.5f)));
    }
	
	void OnTriggerEnter(Collider other)
    {
        if (wasActivated)
        {
            return;
        }
        if(other.tag == "Player")
        {
            wasActivated = true;
            //Debug.Log("spawn location = " + (transform.position - (transform.right * 1)));
        }
    }
}
