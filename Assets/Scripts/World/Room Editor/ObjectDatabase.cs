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
    List<Object> walls = new List<Object>();
    List<Object> outerCornors = new List<Object>();
    List<Object> innerCornors = new List<Object>();
    List<Object> floors = new List<Object>();
    List<Object> doors = new List<Object>();

    List<Object> small = new List<Object>();
    List<Object> medium = new List<Object>();
    List<Object> large = new List<Object>();
    List<Object> xLarge = new List<Object>();

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
        walls = new List<Object>(Resources.LoadAll("ObjectDatabase/Walls"));
        outerCornors = new List<Object>(Resources.LoadAll("ObjectDatabase/OuterCornors"));
        innerCornors = new List<Object>(Resources.LoadAll("ObjectDatabase/InnerCornors"));
        floors = new List<Object>(Resources.LoadAll("ObjectDatabase/Floors"));
        doors = new List<Object>(Resources.LoadAll("ObjectDatabase/Doors"));

        small = new List<Object>(Resources.LoadAll("ObjectDatabase/EnviromentalObjects/Small"));
        medium = new List<Object>(Resources.LoadAll("ObjectDatabase/EnviromentalObjects/Medium"));
        large = new List<Object>(Resources.LoadAll("ObjectDatabase/EnviromentalObjects/Large"));
        xLarge = new List<Object>(Resources.LoadAll("ObjectDatabase/EnviromentalObjects/XLarge"));
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

    public List<Object> GetWalls()
    {
        return walls;
    }

    public List<Object> GetFloors()
    {
        return floors;
    }

    public List<Object> GetOuterCornors()
    {
        return outerCornors;
    }

    public List<Object> GetInnerCornors()
    {
        return innerCornors;
    }

    public List<Object> GetDoors()
    {
        return doors;
    }

    public List<Object> GetSmall()
    {
        return small;
    }

    public List<Object> GetMedium()
    {
        return medium;
    }

    public List<Object> GetLarge()
    {
        return large;
    }
    public List<Object> GetXLarge()
    {
        return xLarge;
    }
}
