﻿using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool used = false;
    GameObject connectedTo;
    public GameObject unusedPrefab;
    
    DoorType doorType = DoorType.unused;

	public void ConnectRoom(GameObject go)
    {
        used = true;
        connectedTo = go;
        doorType = DoorType.entrance;
    }

    public void SetExit()
    {
        doorType = DoorType.exit;
    }

    public bool Connected()
    {
        if(doorType == DoorType.exit)
        {
            return false;
        }
        return used;
    }

    public void CheckConnection()
    {
        if (doorType != DoorType.entrance)
        {
            if (doorType != DoorType.exit)
            {
                GameObject go = Instantiate(unusedPrefab) as GameObject;
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
                go.transform.parent = transform.parent;
            }

            Destroy(gameObject);
        }
    }

    public DoorType GetDoorType()
    {
        return doorType;
    }

    public void BreakConnection()
    {
        used = false;
        connectedTo = null;
        doorType = DoorType.unused;
    }
}
public enum DoorType { entrance, exit, unused };