using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomMaker : MonoBehaviour {

    public GameObject baseRoomPrefab;
    //Defualt Objects
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    public GameObject wallprefab;
    public GameObject innerCornorPrefab;
    public GameObject outerCornorPrefab;
    public GameObject doorPrefab;

    public GameObject hazardObject;

    public GameObject switchObject;

    public GameObject smallObect;
    public GameObject mediumObject;
    public GameObject largeObject;
    public GameObject XLargeObject;

    public GameObject fuel;
    public GameObject comic;

    Room currentRoom;

    string rName;

    public bool EditingRoom()
    {
        return currentRoom != null;
    }

    public Room GetRoom()
    {
        return currentRoom;
    }

    public void LoadRoom(GameObject prefabToLoad)
    {
        GameObject go = Instantiate(prefabToLoad) as GameObject;
        currentRoom = go.GetComponent<Room>();
        rName = prefabToLoad.name;
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

    public void CreateNewRoom()
    {
        if (currentRoom != null)
        {
            Debug.LogError("There already was a room, now you have 2 and that is bad!");
        }
        GameObject go = Instantiate(baseRoomPrefab) as GameObject;
        currentRoom = go.GetComponent<Room>();
        rName = null;
    }

    public GameObject NewFloor(Vector3 pos)
    {
        GameObject temp = Instantiate(floorPrefab, pos, Quaternion.identity) as GameObject;
        GameObject rTemp = Instantiate(roofPrefab, pos + new Vector3(0, 4, 0), roofPrefab.transform.rotation) as GameObject;
        currentRoom.AddShapingObject(temp);
        rTemp.transform.parent = temp.transform;
        return temp;
    }

    public GameObject NewInner(Vector3 pos)
    {
        GameObject temp = Instantiate(innerCornorPrefab, pos, Quaternion.identity) as GameObject;
        currentRoom.AddShapingObject(temp);
        return temp;
    }

    public GameObject NewOuter(Vector3 pos)
    {
        GameObject temp = Instantiate(outerCornorPrefab, pos, Quaternion.identity) as GameObject;
        currentRoom.AddShapingObject(temp);
        return temp;
    }

    public GameObject NewWall(Vector3 pos)
    {
        GameObject temp = Instantiate(wallprefab, pos, Quaternion.identity) as GameObject;
        currentRoom.AddShapingObject(temp);
        return temp;
    }

    public GameObject NewDoor(Vector3 pos)
    {
        GameObject temp = Instantiate(doorPrefab, pos, Quaternion.identity) as GameObject;
        currentRoom.AddDoor(temp);
        return temp;
    }

    public GameObject NewHazard(Vector3 pos, Quaternion rot)
    {
        GameObject temp = Instantiate(hazardObject, pos, rot) as GameObject;
        currentRoom.AddHazardObject(temp);
        return temp;
    }

    public GameObject NewSwitch(Vector3 pos, Quaternion rot)
    {
        GameObject temp = Instantiate(switchObject, pos, rot) as GameObject;
        currentRoom.AddSwitch(temp);
        return temp;
    }

    public GameObject NewProp(Vector3 pos, int size)
    {
        GameObject temp = null;
        switch (size)
        {
            case 0:
                temp = Instantiate(smallObect, pos, Quaternion.identity) as GameObject;
                break;
            case 1:
                temp = Instantiate(mediumObject, pos, Quaternion.identity) as GameObject;
                break;
            case 2:
                temp = Instantiate(largeObject, pos, Quaternion.identity) as GameObject;
                break;
            case 3:
                temp = Instantiate(XLargeObject, pos, Quaternion.identity) as GameObject;
                break;
            default:
                break;
        }
        currentRoom.AddEnviromentalObject(temp);
        return temp;
    }

    public GameObject NewPickUp(Vector3 pos, int type)
    {
        GameObject temp = null;
        if (type == 0)
        {
            temp = Instantiate(fuel, pos, Quaternion.identity) as GameObject;
        }
        else
        {
            temp = Instantiate(comic, pos, Quaternion.identity) as GameObject;
        }
        currentRoom.AddPickup(temp);
        return temp;
    }

}
