using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    //Parents
    public Transform enviromentalObjectsParent;
    public Transform floatingObjectParent;
    public Transform hazardObjectParent;
    public Transform switchParent;
    public Transform shapingParent;
    public Transform pickupParent;
    public Transform doorParent;
    public Transform decorParent;

    //Lists of the objects in the room.
    public List<GameObject> enviromentalObjects;
    public List<GameObject> floatingObjects;
    public List<GameObject> hazardObjects;
    public List<GameObject> switchObjects;
    public List<GameObject> shapingObjects;
    public List<GameObject> pickupObjects;
    public List<GameObject> doorObjects;

    public List<HazardState> hazards;

    public InRoomDoor exitDoor;

    public void SetStatic()
    {
        StaticBatchingUtility.Combine(enviromentalObjectsParent.gameObject);
        //StaticBatchingUtility.Combine(hazardObjectParent.gameObject);
        StaticBatchingUtility.Combine(switchParent.gameObject);
        StaticBatchingUtility.Combine(pickupParent.gameObject);
        StaticBatchingUtility.Combine(decorParent.gameObject);
        //StaticBatchingUtility.Combine(doorParent.gameObject);
    }

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
    public void AddHazardObject(GameObject go)
    {
        if (hazardObjects == null)
        {
            hazardObjects = new List<GameObject>();
        }
        go.transform.parent = hazardObjectParent;
        hazardObjects.Add(go);
    }

    public void AddSwitch(GameObject go)
    {
        if (switchObjects == null)
        {
            switchObjects = new List<GameObject>();
        }
        go.transform.parent = switchParent;
        switchObjects.Add(go);
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

    public void AddDecorObject(GameObject go)
    {
        go.transform.parent = decorParent;
    }

    /// <summary>
    /// Cleans the lists, removes null entries.
    /// </summary>
    public void CleanData()
    {
        CleanList(enviromentalObjects, enviromentalObjectsParent);
        CleanList(floatingObjects, floatingObjectParent);
        CleanList(hazardObjects, hazardObjectParent);
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
        if(pickupObjects == null)
        {
            Debug.LogError("No pickups");
            return false;
        }
        if(pickupObjects.Count < 2)
        {
            Debug.LogError("Needs to have atleast 1 fuel pickup and 1 collectible");
            return false;
        }
        else
        {
            string pickupName = pickupObjects[0].name.Substring(0,3);
            bool doesNotHaveBoth = true;
            foreach (GameObject item in pickupObjects)
            {
                if(item.name.Substring(0,3) != pickupName)
                {
                    doesNotHaveBoth = false;
                }
            }
            if (doesNotHaveBoth)
            {
                Debug.LogError("Needs to have atleast 1 fuel pickup and 1 collectible");
                return false;
            }
        }
        return true;
    }

    public void AddHazard(HazardState HS)
    {
        hazards.Add(HS);
    }

    public void SwitchWasTouched()
    {
        foreach (HazardState item in hazards)
        {
            item.EnabledOrDisableTrap();
        }
        if(exitDoor != null)
        {
            exitDoor.doorToLock.Switch();
        }
    }

    public void SetExitDoor(InRoomDoor ird)
    {
        exitDoor = ird;
    }

    public void EnteredThisRoom()
    {
        if (Debug.isDebugBuild)
        {
            GameObject.FindObjectOfType<DebugSeedText>().EnteredRoom(gameObject.name);
        }
        if(exitDoor != null)
        {
            exitDoor.PrepNextRoom();
        }
    }

    public void PrepRoom()
    {
        gameObject.SetActive(true);
    }

    public void LeftThisRoom()
    {
        gameObject.SetActive(false);
    }

    public void UpdateLists()
    {
        foreach (Transform child in doorParent.transform)
        {
            if (!doorObjects.Contains(child.gameObject))
            {
                AddDoor(child.gameObject);
            }
        }

        foreach (Transform child in shapingParent.transform)
        {
            if (!shapingObjects.Contains(child.gameObject))
            {
                AddShapingObject(child.gameObject);
            }
        }

        foreach (Transform child in hazardObjectParent.transform)
        {
            if (!hazardObjects.Contains(child.gameObject))
            {
                AddHazardObject(child.gameObject);
            }
        }

        foreach (Transform child in switchParent.transform)
        {
            if (!switchObjects.Contains(child.gameObject))
            {
                AddSwitch(child.gameObject);
            }
        }

        foreach (Transform child in pickupParent.transform)
        {
            if (!pickupObjects.Contains(child.gameObject))
            {
                AddPickup(child.gameObject);
            }
        }

        foreach (Transform child in enviromentalObjectsParent.transform)
        {
            if (!enviromentalObjects.Contains(child.gameObject))
            {
                AddEnviromentalObject(child.gameObject);
            }
        }

        foreach (Transform child in floatingObjectParent.transform)
        {
            if (!floatingObjects.Contains(child.gameObject))
            {
                AddFloatingObject(child.gameObject);
            }
        }
    }

    /// <summary>
    /// Randomizes all the interior objects in this room, that is not locked.
    /// </summary>
    public void RandomizeInterior()
    {
        foreach (GameObject item in floatingObjects)
        {
            
            if (item.GetComponent<FloatingProps>() != null)
            {
                item.GetComponent<FloatingProps>().Replace(this);
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in hazardObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace(this);
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in enviromentalObjects)
        {
            if (item.GetComponent<SmallObjectSelector>() != null)
            {
                item.GetComponent<SmallObjectSelector>().Replace(this);
            }
            else if (item.GetComponent<MediumObjectSelector>() != null)
            {
                item.GetComponent<MediumObjectSelector>().Replace(this);
            }
            else if (item.GetComponent<LargeObjectSelector>() != null)
            {
                item.GetComponent<LargeObjectSelector>().Replace(this);
            }
            else if (item.GetComponent<XLargeObjectSelector>() != null)
            {
                item.GetComponent<XLargeObjectSelector>().Replace(this);
            }
            else
            {
                Debug.Log("not there");
            }
        }
        foreach (GameObject item in switchObjects)
        {
           /* if (item.transform.childCount > 0) {
                if (item.transform.GetChild(0).GetComponent<SwitchItem>() != null)
                {
                    item.transform.GetChild(0).GetComponent<SwitchItem>().AssignRoom(this);
                }
            }*/
            if(item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace(this);
            }
        }
        /*foreach (GameObject item in shapingObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace(this);
            }
            else
            {
                Debug.Log("not there");
            }
        }*/
        foreach (GameObject item in pickupObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                item.GetComponent<ObjectSelector>().Replace(this);
            }
            else
            {
                Debug.Log("not there");
            }
        }
    }
}
