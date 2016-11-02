using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour
{

    //Prefabs, needs to get these from a database.
    public GameObject baseRoomPrefab;
    public GameObject staticObject;
    public GameObject dynamicObject;
    public GameObject envirionmentObject;

    Room currentRoom;

    /// <summary>
    /// Creates a new room, if there is not already one.
    /// </summary>
	public void CreateNewRoom()
    {
        GameObject go = Instantiate(baseRoomPrefab) as GameObject;
        currentRoom = go.GetComponent<Room>();
    }

    public void LoadRoom(GameObject prefabToLoad)
    {
        GameObject go = Instantiate(prefabToLoad) as GameObject;
        currentRoom = go.GetComponent<Room>();
    }

    public bool EditingRoom()
    {
        return currentRoom != null;
    }

    public Room GetRoom()
    {
        return currentRoom;
    }

    public void AddNewStaticObject()
    {
        GameObject temp = Instantiate(staticObject) as GameObject;
        currentRoom.AddStaticObject(temp);
    }

    public void AddNewDynamicObject()
    {
        GameObject temp = Instantiate(dynamicObject) as GameObject;
        currentRoom.AddDynamicObject(temp);
    }

    public void AddNewEnvironmentObject()
    {
        GameObject temp = Instantiate(envirionmentObject) as GameObject;
        currentRoom.AddEnvirionmentalObject(temp);
    }

    public void DestroyRoom()
    {
        if (Application.isEditor)
        {
            DestroyImmediate(currentRoom.gameObject);
        }
        else
        {
            Destroy(currentRoom.gameObject);
        }
    }
}
