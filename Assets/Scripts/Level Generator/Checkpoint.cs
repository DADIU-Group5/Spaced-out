﻿using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    bool wasActivated = false;
    public InRoomDoor door;
	
	void OnTriggerEnter(Collider other)
    {
        if (wasActivated)
        {
            return;
        }
        if(other.tag == "Player")
        {
            Triggered();
            wasActivated = true;
            CheckpointManager.instance.SetNewCheckpoint(transform.position);
            CheckpointManager.instance.SetNewCheckpointRotation(transform.right);
            CheckpointManager.instance.SetFuelCount(other.GetComponent<FuelController>().GetCurrentFuel());
        }
    }

    void Triggered()
    {
        door.EnteredNextRoom();
    }
}
