using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectDatabase : MonoBehaviour {

    public static ObjectDatabase instance;

    List<Object> enviromentalObjects = new List<Object>();
    List<Object> floatingObjects = new List<Object>();
    List<Object> staticObjects = new List<Object>();
    List<Object> shapingObjects = new List<Object>();
    List<Object> pickupObjects = new List<Object>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There are 2 ObjectDatabases, destroyed the new one.");
            Destroy(gameObject);
            return;
        }
        LoadObjects();
    }

    #if UNITY_EDITOR
    //Only used in the editor, to make sure this is a singleton, only an issue that it 'forgets' itself when in editor, not when the game is running.
    void Update()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endif
    
    /// <summary>
    /// Loads the objects from the resources folder.
    /// </summary>
    public void LoadObjects()
    {
        enviromentalObjects = new List<Object>(Resources.LoadAll("ObjectDatabase/EnviromentalObjects"));
        floatingObjects = new List<Object>(Resources.LoadAll("ObjectDatabase/FloatingObjects"));
        staticObjects = new List<Object>(Resources.LoadAll("ObjectDatabase/StaticObjects"));
        shapingObjects = new List<Object>(Resources.LoadAll("ObjectDatabase/ShapingObjects"));
        pickupObjects = new List<Object>(Resources.LoadAll("ObjectDatabase/Pickups"));
    }

    public List<Object> GetEnviromentalObjects()
    {
        return enviromentalObjects;
    }

    public List<Object> GetFloatingObjects()
    {
        return floatingObjects;
    }

    public List<Object> GetStaticObjects()
    {
        return staticObjects;
    }

    public List<Object> GetShapingObjects()
    {
        return shapingObjects;
    }

    public List<Object> GetPickupObjects()
    {
        return pickupObjects;
    }

}
