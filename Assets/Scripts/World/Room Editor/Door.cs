using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool used = false;
    GameObject connectedTo;
    public GameObject unusedPrefab;
    
    public DoorType doorType = DoorType.unused;

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

    public void SetEntry()
    {
        doorType = DoorType.entryhall;
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
                go.transform.parent = transform.parent;
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
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

    public void SetDoorBehindKey()
    {
        doorType = DoorType.Key;
    }
}
public enum DoorType { entrance, exit, unused, Key, entryhall };