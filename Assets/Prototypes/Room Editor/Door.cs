using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool used = false;
    GameObject connectedTo;

	public void ConnectRoom(GameObject go)
    {
        used = true;
        connectedTo = go;
    }

    public bool Connected()
    {
        return used;
    }
}
