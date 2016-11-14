using UnityEngine;
using System.Collections;

public class RoomCreator : MonoBehaviour
{
    //Prefabs, needs to get these from a database.
    public GameObject baseRoomPrefab;
    public GameObject environmentalObject;
    public GameObject floatingObject;
    public GameObject staticObject;
    public GameObject shapingObject;
    public GameObject pickupObject;
    public GameObject DoorObject;

    Room currentRoom;

    string rName;

    /// <summary>
    /// Creates a new room, if there is not already one.
    /// </summary>
	public void CreateNewRoom()
    {
        if(currentRoom != null)
        {
            Debug.LogError("There already was a room, now you have 2 and that is bad!");
        }
        GameObject go = Instantiate(baseRoomPrefab) as GameObject;
        currentRoom = go.GetComponent<Room>();
        rName = null;
    }

    public void LoadRoom(GameObject prefabToLoad)
    {
        GameObject go = Instantiate(prefabToLoad) as GameObject;
        currentRoom = go.GetComponent<Room>();
        rName = prefabToLoad.name;
    }

    public bool EditingRoom()
    {
        return currentRoom != null;
    }

    public Room GetRoom()
    {
        return currentRoom;
    }

    public GameObject AddNewEnvironmentalObject()
    {
        GameObject temp = Instantiate(environmentalObject) as GameObject;
        currentRoom.AddEnviromentalObject(temp);
        return temp;
    }

    public GameObject AddFloatingObject()
    {
        GameObject temp = Instantiate(floatingObject) as GameObject;
        currentRoom.AddFloatingObject(temp);
        return temp;
    }

    public GameObject AddStaticObject()
    {
        GameObject temp = Instantiate(staticObject) as GameObject;
        currentRoom.AddStaticObject(temp);
        return temp;
    }

    public GameObject AddNewshapingObject()
    {
        GameObject temp = Instantiate(shapingObject) as GameObject;
        currentRoom.AddShapingObject(temp);
        return temp;
    }

    public GameObject AddNewPickupObject()
    {
        GameObject temp = Instantiate(pickupObject) as GameObject;
        currentRoom.AddPickup(temp);
        return temp;
    }

    public GameObject AddNewDoor()
    {
        GameObject temp = Instantiate(DoorObject) as GameObject;
        currentRoom.AddDoor(temp);
        return temp;
    }

    public string GetName()
    {
        return rName;
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
