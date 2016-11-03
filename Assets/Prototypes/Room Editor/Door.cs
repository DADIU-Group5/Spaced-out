using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    bool used = false;
    GameObject connectedTo;
    public GameObject unusedPrefab;

	public void ConnectRoom(GameObject go)
    {
        used = true;
        connectedTo = go;
    }

    public bool Connected()
    {
        return used;
    }

    public void CheckConnection()
    {
        if (!used)
        {
            GameObject go = Instantiate(unusedPrefab) as GameObject;
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.transform.parent = transform.parent;

            Destroy(gameObject);
        }
    }
}
