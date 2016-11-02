using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{

    //Parents
    public Transform staticObjectsParent;
    public Transform dynamicObjectsParent;
    public Transform envirionmentParent;

    //Lists
    List<GameObject> staticObjects = new List<GameObject>();
    List<GameObject> dynamicObjects = new List<GameObject>();
    List<GameObject> envirionmentalObject = new List<GameObject>();

    public void AddStaticObject(GameObject go)
    {
        go.transform.parent = staticObjectsParent;
        staticObjects.Add(go);
    }

    public void AddDynamicObject(GameObject go)
    {
        go.transform.parent = dynamicObjectsParent;
        dynamicObjects.Add(go);
    }

    public void AddEnvirionmentalObject(GameObject go)
    {
        go.transform.parent = envirionmentParent;
        envirionmentalObject.Add(go);
    }
}
