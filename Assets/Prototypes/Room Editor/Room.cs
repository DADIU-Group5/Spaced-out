using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    //Parents
    public Transform enviromentalObjectsParent;
    public Transform dynamicObjectsParent;
    public Transform shapingParent;
    public Transform doorParent;

    //Lists of the objects in the room.
    public List<GameObject> enviromentalObjects;
    public List<GameObject> dynamicObjects;
    public List<GameObject> shapingObject;
    public List<GameObject> doorObject;

    /// <summary>
    /// Adds a new enviromental object.
    /// </summary>
    /// <param name="go"></param>
    public void AddEnviromentalObject(GameObject go)
    {
        if(enviromentalObjects == null)
        {
            enviromentalObjects = new List<GameObject>();
        }
        go.transform.parent = enviromentalObjectsParent;
        enviromentalObjects.Add(go);
    }

    /// <summary>
    /// Adds a new dynamic object.
    /// </summary>
    /// <param name="go"></param>
    public void AddDynamicObject(GameObject go)
    {
        if (dynamicObjects == null)
        {
            dynamicObjects = new List<GameObject>();
        }
        go.transform.parent = dynamicObjectsParent;
        dynamicObjects.Add(go);
    }

    /// <summary>
    /// Adds a new shaping object.
    /// </summary>
    /// <param name="go"></param>
    public void AddShapingObject(GameObject go)
    {
        if (shapingObject == null)
        {
            shapingObject = new List<GameObject>();
        }
        go.transform.parent = shapingParent;
        shapingObject.Add(go);
    }

    /// <summary>
    /// Adds a new door.
    /// </summary>
    /// <param name="go"></param>
    public void AddDoor(GameObject go)
    {
        if (doorObject == null)
        {
            doorObject = new List<GameObject>();
        }
        go.transform.parent = doorParent;
        doorObject.Add(go);
    }

    /// <summary>
    /// Cleans the lists, removes null entries.
    /// </summary>
    public void CleanData()
    {
        CleanList(enviromentalObjects);
        CleanList(dynamicObjects);
        CleanList(shapingObject);
        CleanList(doorObject);
    }

    /// <summary>
    /// Method for cleaning a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="toClean"></param>
    void CleanList<T>(List<T> toClean)
    {
        for (int i = toClean.Count-1; i >= 0 ; i--)
        {
            if(toClean[i] == null)
            {
                toClean.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Checks if the room follows the rules that a room should have.
    /// </summary>
    /// <returns></returns>
    public bool canBeRoom()
    {
        if(doorObject == null)
        {
            Debug.LogError("No doors");
            return false;
        }
        if(doorObject.Count < 2)
        {
            Debug.LogError("Room needs atleast 2 doors!");
            return false;
        }
        return true;
    }
}
