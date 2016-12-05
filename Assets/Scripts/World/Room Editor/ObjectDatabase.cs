using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ObjectDatabase : MonoBehaviour {

    public static ObjectDatabase instance;

    //List<GameObject> enviromentalObjects = new List<GameObject>();
    List<GameObject> floatingObjects = new List<GameObject>();
    List<GameObject> staticObjects = new List<GameObject>();
    List<GameObject> shapingObjects = new List<GameObject>();
    List<GameObject> fuel = new List<GameObject>();
    List<GameObject> comics = new List<GameObject>();
    List<GameObject> walls = new List<GameObject>();
    List<GameObject> outerCornors = new List<GameObject>();
    List<GameObject> innerCornors = new List<GameObject>();
    List<GameObject> floors = new List<GameObject>();
    List<GameObject> doors = new List<GameObject>();
    List<GameObject> barrels = new List<GameObject>();
    List<GameObject> switchObj = new List<GameObject>();

    List<GameObject> small = new List<GameObject>();
    List<GameObject> medium = new List<GameObject>();
    List<GameObject> large = new List<GameObject>();
    List<GameObject> xLarge = new List<GameObject>();

    public Themes levelTheme;

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
        //GET THE THEME FOR THE LEVEL!
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
        
        if(GenerationDataManager.instance != null)
        {
            levelTheme = GenerationDataManager.instance.GetLevelData().roomTheme;
        }
        
        floatingObjects = LoadFromPathFiltered("ObjectDatabase/FloatingObjects");
        staticObjects = LoadFromPathFiltered("ObjectDatabase/StaticObjects");
        shapingObjects = LoadFromPathFiltered("ObjectDatabase/ShapingObjects");
        fuel = LoadFromPathFiltered("ObjectDatabase/Pickups/Fuel");
        comics = LoadFromPathFiltered("ObjectDatabase/Pickups/Comics");

        walls = LoadFromPathFiltered("ObjectDatabase/Walls");
        outerCornors = LoadFromPathFiltered("ObjectDatabase/OuterCornors");
        innerCornors = LoadFromPathFiltered("ObjectDatabase/InnerCornors");
        floors = LoadFromPathFiltered("ObjectDatabase/Floors");
        doors = LoadFromPathFiltered("ObjectDatabase/Doors");
        barrels = LoadFromPathFiltered("ObjectDatabase/ExplodingBarrel");
        switchObj = LoadFromPathFiltered("ObjectDatabase/Switch");

        small = LoadFromPathFiltered("ObjectDatabase/EnviromentalObjects/Small");
        medium = LoadFromPathFiltered("ObjectDatabase/EnviromentalObjects/Medium");
        large = LoadFromPathFiltered("ObjectDatabase/EnviromentalObjects/Large");
        xLarge = LoadFromPathFiltered("ObjectDatabase/EnviromentalObjects/XLarge");
    }

    List<GameObject> LoadFromPathFiltered(string path)
    {
        List<GameObject> toReturn = new List<GameObject>(Resources.LoadAll<GameObject>(path));
        for (int i = toReturn.Count-1; i >= 0; i--)
        {
            if(toReturn[i].GetComponent<ThemeObject>() != null)
            {
                if(toReturn[i].GetComponent<ThemeObject>().GetTheme() != levelTheme)
                {
                    toReturn.RemoveAt(i);
                }
            }
        }
        return toReturn;
    }

    /*public List<GameObject> GetEnviromentalObjects()
    {
        return enviromentalObjects;
    }*/

    public List<GameObject> GetFloatingObjects()
    {
        return floatingObjects;
    }

    public List<GameObject> GetStaticObjects()
    {
        return staticObjects;
    }

    public List<GameObject> GetShapingObjects()
    {
        return shapingObjects;
    }

    public List<GameObject> GetFuel()
    {
        return fuel;
    }

    public List<GameObject> GetComic()
    {
        return comics;
    }

    public List<GameObject> GetBarrel()
    {
        return barrels;
    }

    public List<GameObject> GetSwitch()
    {
        return switchObj;
    }

    public List<GameObject> GetWalls()
    {
        return walls;
    }

    public List<GameObject> GetFloors()
    {
        return floors;
    }

    public List<GameObject> GetOuterCornors()
    {
        return outerCornors;
    }

    public List<GameObject> GetInnerCornors()
    {
        return innerCornors;
    }

    public List<GameObject> GetDoors()
    {
        return doors;
    }

    public List<GameObject> GetSmall()
    {
        return small;
    }

    public List<GameObject> GetMedium()
    {
        return medium;
    }

    public List<GameObject> GetLarge()
    {
        return large;
    }
    public List<GameObject> GetXLarge()
    {
        return xLarge;
    }
}

public enum Themes
{
    Storage, Botanic, Cafeteria, ScienceLab
}