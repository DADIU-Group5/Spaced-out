using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomFixer : MonoBehaviour
{

    Room room;
    RoomMaker RM;

    void Setup()
    {
        RM = GameObject.FindObjectOfType<RoomMaker>();
        room = GetComponent<Room>();
    }

    public void Fix()
    {
        Setup();
        //FixSwitch();
        //FixPickups();
        /*MakeStatic(room.shapingParent);
        MakeStatic(room.switchParent);
        MakeStatic(room.enviromentalObjectsParent);
        MakeStatic(room.doorParent);
        MakeStatic(room.pickupParent);
        MakeStatic(GameObject.Find("Decor").transform);
        if(room.decorParent == null)
        {
            room.decorParent = GameObject.Find("Decor").transform;
        }*/
        //FixSize(room.shapingParent);

        FindMisplacedObjects();
        ReplaceAllShaping();
        room.CleanData();
    }

    void ReplaceAllShaping()
    {
        int count = room.shapingParent.childCount;
        for (int i = count-1; i >= 0; i--)
        {
            string shortName = room.shapingParent.GetChild(i).name.Substring(0, 4);
            switch (shortName)
            {
                case "Wall":
                    RM.RotateObject(RM.NewWall(room.shapingParent.GetChild(i).position), room.shapingParent.GetChild(i));
                    DestroyImmediate(room.shapingParent.GetChild(i).gameObject);
                    break;
                case "Inne":
                    RM.RotateObject(RM.NewInner(room.shapingParent.GetChild(i).position), room.shapingParent.GetChild(i));
                    DestroyImmediate(room.shapingParent.GetChild(i).gameObject);
                    break;
                case "Oute":
                    RM.RotateObject(RM.NewOuter(room.shapingParent.GetChild(i).position), room.shapingParent.GetChild(i));
                    DestroyImmediate(room.shapingParent.GetChild(i).gameObject);
                    break;
                case "Floo":
                    RM.RotateObject(RM.NewFloor(room.shapingParent.GetChild(i).position), room.shapingParent.GetChild(i));
                    DestroyImmediate(room.shapingParent.GetChild(i).gameObject);
                    break;
                default:
                    Debug.LogError("AN OBJECT HAS THE WRONG PARENT! " + room.shapingParent.GetChild(i).name);
                    break;
            }
        }
    }

    void FindMisplacedObjects()
    {
        foreach (Transform item in room.enviromentalObjectsParent)
        {
            if (!room.enviromentalObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if(item.GetComponent<XLargeObjectSelector>() == null && item.GetComponent<LargeObjectSelector>() == null && item.GetComponent<MediumObjectSelector>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.floatingObjectParent)
        {
            if (!room.floatingObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<FloatingProps>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.hazardObjectParent)
        {
            if (!room.hazardObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<StaticObjectSelector>() == null && item.GetComponent<BarrelObjectSelector>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.shapingParent)
        {
            if (!room.shapingObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<FloorSelector>() == null && item.GetComponent<OuterCornorSelector>() == null && item.GetComponent<InnerCornorSelector>() == null && item.GetComponent<WallSelector>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.pickupParent)
        {
            if (!room.pickupObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<ComicSelector>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.doorParent)
        {
            if (!room.doorObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<Door>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
        foreach (Transform item in room.switchParent)
        {
            if (!room.switchObjects.Contains(item.gameObject))
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
            if (item.GetComponent<SwitchSelector>() == null)
            {
                Debug.LogError("OBJECT NOT IN CORRECT LIST / OBJECT HAS WRONG PARENT! " + item.name);
            }
        }
    }

    void MakeStatic(Transform parent)
    {
        foreach (Transform item in parent)
        {
            item.gameObject.isStatic = true;
            MakeStatic(item);
        }
        parent.gameObject.isStatic = true;
    }

    void FixSwitch()
    {
        List<GameObject> temp = room.switchObjects;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].GetComponent<SwitchSelector>() == null)
            {
                GameObject holder = temp[i];
                GameObject newObj = Instantiate(RM.switchObject, temp[i].transform.position, temp[i].transform.rotation, temp[i].transform.parent) as GameObject;
                temp[i] = newObj;
                DestroyImmediate(holder);
                Debug.Log("Replaced a switch!");
            }
            else
            {
                Debug.Log("had correct switch");
            }
        }
        room.switchObjects = temp;
    }

    void FixPickups()
    {
        List<GameObject> temp = room.pickupObjects;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].name[0] == 'c' || temp[i].name[0] == 'C')
            {
                if (temp[i].GetComponent<ComicSelector>() == null)
                {
                    GameObject holder = temp[i];
                    GameObject newObj = Instantiate(RM.comic, temp[i].transform.position, temp[i].transform.rotation, temp[i].transform.parent) as GameObject;
                    temp[i] = newObj;
                    DestroyImmediate(holder);
                    Debug.Log("Replaced a comic!");
                }
                else
                {
                    Debug.Log("had correct comic");
                }
            }
            else if (temp[i].name[0] == 'f' || temp[i].name[0] == 'F')
            {
                if (temp[i].GetComponent<FuelSelector>() == null)
                {
                    GameObject holder = temp[i];
                    GameObject newObj = Instantiate(RM.fuel, temp[i].transform.position, temp[i].transform.rotation, temp[i].transform.parent) as GameObject;
                    temp[i] = newObj;
                    DestroyImmediate(holder);
                    Debug.Log("Replaced a fuel!");
                }
                else
                {
                    Debug.Log("had correct fuel");
                }
            }
            else
            {
                Debug.Log("did not start with c or f");
            }
        }
    }

    List<GameObject> FixObject<T>(List<GameObject> listToFix, GameObject replaceWith)
    {
        for (int i = 0; i < listToFix.Count; i++)
        {
            if (listToFix[i].GetComponent<T>() == null)
            {
                GameObject holder = listToFix[i];
                GameObject newObj = Instantiate(RM.switchObject, listToFix[i].transform.position, listToFix[i].transform.rotation, listToFix[i].transform.parent) as GameObject;
                listToFix[i] = newObj;
                DestroyImmediate(holder);
                Debug.Log("Replaced an object, which was missing type: " + typeof(T).ToString());
            }
            else
            {
                Debug.Log("Already had: " + typeof(T).ToString());
            }
        }
        return listToFix;
    }

    void FixSize(Transform parent)
    {
        foreach (Transform item in parent)
        {
            if (item == parent)
            {
                continue;
            }
            if (item.GetComponent<MeshRenderer>() != null || item.name == "Doorway_Sci_Fil2")
            {
                item.localScale = fixScale(item.localScale);
                item.localPosition = FixPos(item.localPosition);
            }

            FixSize(item);
        }
    }

    Vector3 fixScale(Vector3 oldScale)
    {
        float x = oldScale.x;
        float y = oldScale.y;
        float z = oldScale.z;

        if (x == 1)
        {
            x = 1;
        }
        else if (x < 1)
        {
            x = 1.013f / 2f;
        }
        else
        {
            x = 1.013f;
        }

        if (y == 1)
        {
            y = 1;
        }
        else if (y < 1)
        {
            y = 1.013f / 2f;
        }
        else
        {
            y = 1.013f;
        }

        if (z == 1)
        {
            z = 1;
        }
        else if (z < 1)
        {
            z = 1.013f / 2f;
        }
        else
        {
            z = 1.013f;
        }

        return new Vector3(x, y, z);
    }

    Vector3 FixPos(Vector3 oldPos)
    {
        float x = oldPos.x;
        float y = oldPos.y;
        float z = oldPos.z;
        if(oldPos.x == -2.826f)
        {
            x = -2.825f;
        }
       /* else if(oldPos.x == 0.825f)
        {
            x = 0.808f;
        }*/

        /*if(oldPos.z == -0.825f)
        {
            z = -0.808f;
        }*/
        return new Vector3(x, y, z);
    }
}