using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    bool wasActivated = false;
	
	void OnTriggerEnter(Collider other)
    {
        if (wasActivated)
        {
            return;
        }
        if(other.tag == "Player")
        {
            wasActivated = true;
            CheckpointManager.instance.SetNewCheckpoint(transform.position);
            CheckpointManager.instance.SetNewCheckpointRotation(transform.right);
            //Debug.Log("spawn location = " + (transform.position - (transform.right * 1)));
        }
    }
}
