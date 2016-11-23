using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomFixer : MonoBehaviour {

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
        MakeStatic(room.shapingParent);
        MakeStatic(room.switchParent);
        MakeStatic(room.enviromentalObjectsParent);
        MakeStatic(room.doorParent);
        MakeStatic(room.pickupParent);
        MakeStatic(GameObject.Find("Decor").transform);
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
            if(temp[i].name[0] == 'c' || temp[i].name[0] == 'C')
            {
                if (temp[i].GetComponent<ComicSelector>() == null) {
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
            else if(temp[i].name[0] == 'f' || temp[i].name[0] == 'F')
            {
                if (temp[i].GetComponent<FuelSelector>() == null) {
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
            if(listToFix[i].GetComponent<T>() == null)
            {
                GameObject holder = listToFix[i];
                GameObject newObj = Instantiate(RM.switchObject, listToFix[i].transform.position, listToFix[i].transform.rotation, listToFix[i].transform.parent) as GameObject;
                listToFix[i] = newObj;
                DestroyImmediate(holder);
                Debug.Log("Replaced an object, which was missing type: "+typeof(T).ToString());
            }
            else
            {
                Debug.Log("Already had: " + typeof(T).ToString());
            }
        }
        return listToFix; ;
    }
}
