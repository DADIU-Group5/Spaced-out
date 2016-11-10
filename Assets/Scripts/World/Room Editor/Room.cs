using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    //Parents
    public Transform enviromentalObjectsParent;
    public Transform floatingObjectParent;
    public Transform staticObjectParent;
    public Transform shapingParent;
    public Transform pickupParent;
    public Transform doorParent;

    //Lists of the objects in the room.
    public List<GameObject> enviromentalObjects;
    public List<GameObject> floatingObjects;
    public List<GameObject> staticObjects;
    public List<GameObject> shapingObjects;
    public List<GameObject> pickupObjects;
    public List<GameObject> doorObjects;

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
    public void AddFloatingObject(GameObject go)
    {
        if (floatingObjects == null)
        {
            floatingObjects = new List<GameObject>();
        }
        go.transform.parent = floatingObjectParent;
        floatingObjects.Add(go);
    }

    /// <summary>
    /// Adds a new dynamic object.
    /// </summary>
    /// <param name="go"></param>
    public void AddStaticObject(GameObject go)
    {
        if (staticObjects == null)
        {
            staticObjects = new List<GameObject>();
        }
        go.transform.parent = staticObjectParent;
        staticObjects.Add(go);
    }

    /// <summary>
    /// Adds a new shaping object.
    /// </summary>
    /// <param name="go"></param>
    public void AddShapingObject(GameObject go)
    {
        if (shapingObjects == null)
        {
            shapingObjects = new List<GameObject>();
        }
        go.transform.parent = shapingParent;
        shapingObjects.Add(go);
    }

    /// <summary>
    /// Adds a new pickup.
    /// </summary>
    /// <param name="go"></param>
    public void AddPickup(GameObject go)
    {
        if (pickupObjects == null)
        {
            pickupObjects = new List<GameObject>();
        }
        go.transform.parent = pickupParent;
        pickupObjects.Add(go);
    }

    /// <summary>
    /// Adds a new door.
    /// </summary>
    /// <param name="go"></param>
    public void AddDoor(GameObject go)
    {
        if (doorObjects == null)
        {
            doorObjects = new List<GameObject>();
        }
        go.transform.parent = doorParent;
        doorObjects.Add(go);
    }

    /// <summary>
    /// Cleans the lists, removes null entries.
    /// </summary>
    public void CleanData()
    {
        CleanList(enviromentalObjects, enviromentalObjectsParent);
        CleanList(floatingObjects, floatingObjectParent);
        CleanList(staticObjects, staticObjectParent);
        CleanList(shapingObjects, shapingParent);
        CleanList(pickupObjects, pickupParent);
        CleanList(doorObjects, doorParent);
    }

    /// <summary>
    /// Method for cleaning a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="toClean"></param>
    void CleanList(List<GameObject> toClean, Transform t)
    {
        for (int i = toClean.Count-1; i >= 0 ; i--)
        {
            if(toClean[i] == null || toClean[i].transform.parent != t)
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
        if(doorObjects == null)
        {
            Debug.LogError("No doors");
            return false;
        }
        if(doorObjects.Count < 2)
        {
            Debug.LogError("Room needs atleast 2 doors!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Randomizes all the interior objects in this room, that is not locked.
    /// </summary>
    public void RandomizeInterior()
    {
        foreach (GameObject item in floatingObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace();
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in staticObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace();
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in enviromentalObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace();
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in shapingObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace();
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in pickupObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace();
            }
            else
            {
                Debug.Log("not there");
            }
        }
    }
}
