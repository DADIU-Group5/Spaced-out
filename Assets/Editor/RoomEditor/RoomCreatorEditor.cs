using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*
 * NO LONGER USED! 
 * USES ROOMEDITORWINDOW INSTEAD!
 */


[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    RoomCreator RC;

    enum States { noRoom, Editing, Saving, Loading }
    States states;

    string roomName;

    string pathName = "Assets/Resources/Rooms/";
    
    int LoadID = 0;
    List<Object> rooms;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("YOU NEED TO USE THE 'ROOM EDITOR WINDOW!",EditorStyles.helpBox);
        RC = (RoomCreator)target;
        DrawDefaultInspector();
        return;

        /*if (rooms == null || rooms.Count == 0)
        {
            LoadRooms();
        }
        GetState();
        switch (states)
        {
            case States.noRoom:
                NoRoom();
                break;
            case States.Editing:
                EditingRoom();
                break;
            case States.Saving:
                SavingRoom();
                break;
            case States.Loading:
                Loading();
                break;
            default:
                break;
        }*/
    }

    void GetState()
    {
        if (!RC.EditingRoom())
        {
            if (states == States.Loading)
            {
                return;
            }
            states = States.noRoom;
        }
        else
        {
            if (states == States.Saving)
            {
                return;
            }
            states = States.Editing;
        }
    }

    void NoRoom()
    {
        if (GUILayout.Button("Create Room"))
        {
            states = States.Editing;
            LoadRooms();
            roomName = "";
            RC.CreateNewRoom();
        }
        if (GUILayout.Button("Load Room"))
        {
            states = States.Loading;
        }
    }

    void EditingRoom()
    {
        if (GUILayout.Button("Create Environmental Object"))
        {
            RC.AddNewEnvironmentalObject();
        }
        if (GUILayout.Button("Create Floating Object"))
        {
            RC.AddFloatingObject();
        }
        if (GUILayout.Button("Create Static Object"))
        {
            RC.AddStaticObject();
        }
        if (GUILayout.Button("Create Shaping Object"))
        {
            RC.AddNewshapingObject();
        }
        if (GUILayout.Button("Create Door"))
        {
            RC.AddNewDoor();
        }
        if (GUILayout.Button("Clear!") && EditorUtility.DisplayDialog("Clear?", "Are you sure you want to clear the current room? Will erase any unsaved changes!", "I am sure", "Cancel"))
        {
            RC.DestroyRoom();
        }
        if (GUILayout.Button("Save!"))
        {
            RC.GetRoom().CleanData();
            if (RC.GetRoom().canBeRoom())
            {
                states = States.Saving;
            }
        }
        if (GUILayout.Button("Update All"))
        {
            UpdateAll();
        }
    }

    void SavingRoom()
    {
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        if (GUILayout.Button("Temporary save!"))
        {
            if (SaveObject())
            {
                states = States.Editing;
            }
        }
        if (GUILayout.Button("Save and finalize!"))
        {
            if (SaveObject())
            {
                RC.DestroyRoom();
            }
        }
        if (GUILayout.Button("Back"))
        {
            states = States.Editing;
        }
    }

    bool SaveObject()
    {
        if (roomName == "")
        {
            EditorUtility.DisplayDialog("Invalid name", "The name: \"" + roomName + "\" is not allowed!", "Back");
            return false;
        }
        if (AssetWithNameExists(roomName))
        {
            return false;
        }
        else
        {
            Room roomToSave = RC.GetRoom();
            PrefabUtility.CreatePrefab(pathName + roomName + ".prefab", roomToSave.gameObject);
            AssetDatabase.ImportAsset(pathName + roomName + ".prefab");
            AssetDatabase.Refresh();
            LoadRooms();
            return true;
        }
    }

    bool AssetWithNameExists(string _name)
    {
        if (rooms == null)
        {
            return false;
        }
        foreach (UnityEngine.Object item in rooms)
        {
            if (item.name == _name)
            {
                if (EditorUtility.DisplayDialog("Name already in use", "Name already in use, overwrite? or rename?", "Overwrite", "Rename"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }

    void Loading()
    {
        LoadRooms();
        EditorGUILayout.LabelField("Room count:", rooms.Count.ToString());
        EditorGUILayout.LabelField("Room:", LoadID.ToString());
        if (rooms.Count == 0)
        {
            LoadID = 0;
            GUILayout.Label("There is no saved rooms!");
            if (GUILayout.Button("Back"))
            {
                states = States.noRoom;
            }
            return;
        }
        EditorGUILayout.LabelField("Name", rooms[LoadID].name);
        GUILayout.Label(AssetPreview.GetAssetPreview(rooms[LoadID]));
        if (GUILayout.Button("Load"))
        {
            roomName = rooms[LoadID].name;
            RC.LoadRoom((GameObject)rooms[LoadID]);
        }
        if (GUILayout.Button("Remove!") && EditorUtility.DisplayDialog("DELETE?", "Are you sure you want to delete this room? CANNOT be undone!", "YES!", "Cancel"))
        {

            FileUtil.DeleteFileOrDirectory(pathName + rooms[LoadID].name + ".prefab");
            EditorUtility.UnloadUnusedAssetsImmediate();
            AssetDatabase.Refresh();
            LoadRooms();
            if (LoadID >= rooms.Count)
            {
                LoadID = rooms.Count - 1;
            }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Prev"))
        {
            LoadID--;
            if (LoadID < 0)
            {
                LoadID = rooms.Count - 1;
            }
        }
        if (GUILayout.Button("Next"))
        {
            LoadID++;
            if (LoadID >= rooms.Count)
            {
                LoadID = 0;
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Back"))
        {
            states = States.noRoom;
        }
    }

    void LoadRooms()
    {
        rooms = new List<Object>(Resources.LoadAll("Rooms"));
    }

    /// <summary>
    /// Makes sure there are no children of the parents that aren't in the list.
    /// And makes sure that the objects has the correct components (SHOULD no longer be needed)
    /// </summary>
    void UpdateAll()
    {
        foreach (Transform child in RC.GetRoom().shapingParent.transform)
        {
            if (!RC.GetRoom().shapingObjects.Contains(child.gameObject))
            {
                RC.GetRoom().AddShapingObject(child.gameObject);
            }
        }
        foreach (Transform child in RC.GetRoom().floatingObjectParent.transform)
        {
            if (!RC.GetRoom().floatingObjects.Contains(child.gameObject))
            {
                RC.GetRoom().AddFloatingObject(child.gameObject);
            }
        }
        foreach (Transform child in RC.GetRoom().staticObjectParent.transform)
        {
            if (!RC.GetRoom().staticObjects.Contains(child.gameObject))
            {
                RC.GetRoom().AddStaticObject(child.gameObject);
            }
        }
        foreach (Transform child in RC.GetRoom().enviromentalObjectsParent.transform)
        {
            if (!RC.GetRoom().enviromentalObjects.Contains(child.gameObject))
            {
                RC.GetRoom().AddEnviromentalObject(child.gameObject);
            }
        }

        foreach (GameObject item in RC.GetRoom().floatingObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ObjectSelector>());
            }
            if (item.GetComponent<ShapingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ShapingObjectSelector>());
            }
            if (item.GetComponent<FloatingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<FloatingObjectSelector>());
            }
            if (item.GetComponent<EnviromentalObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<EnviromentalObjectSelector>());
            }

            //Actaul add
            if (item.GetComponent<FloatingObjectSelector>() == null)
            {
                item.AddComponent<FloatingObjectSelector>();
            }
        }

        foreach (GameObject item in RC.GetRoom().staticObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ObjectSelector>());
            }
            if (item.GetComponent<ShapingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ShapingObjectSelector>());
            }
            if (item.GetComponent<FloatingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<FloatingObjectSelector>());
            }
            if (item.GetComponent<StaticObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<StaticObjectSelector>());
            }
            if (item.GetComponent<EnviromentalObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<EnviromentalObjectSelector>());
            }

            //Actaul add
            if (item.GetComponent<StaticObjectSelector>() == null)
            {
                item.AddComponent<StaticObjectSelector>();
            }
        }

        foreach (GameObject item in RC.GetRoom().enviromentalObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ObjectSelector>());
            }
            if (item.GetComponent<ShapingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ShapingObjectSelector>());
            }
            if (item.GetComponent<FloatingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<FloatingObjectSelector>());
            }
            if (item.GetComponent<StaticObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<StaticObjectSelector>());
            }
            if (item.GetComponent<EnviromentalObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<EnviromentalObjectSelector>());
            }

            //Actaul add
            if (item.GetComponent<EnviromentalObjectSelector>() == null)
            {
                item.AddComponent<EnviromentalObjectSelector>();
            }
        }
        foreach (GameObject item in RC.GetRoom().shapingObjects)
        {
            if (item.GetComponent<ObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ObjectSelector>());
            }
            if (item.GetComponent<ShapingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<ShapingObjectSelector>());
            }
            if (item.GetComponent<FloatingObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<FloatingObjectSelector>());
            }
            if (item.GetComponent<StaticObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<StaticObjectSelector>());
            }
            if (item.GetComponent<EnviromentalObjectSelector>() != null)
            {
                DestroyImmediate(item.GetComponent<EnviromentalObjectSelector>());
            }

            //Actaul add
            if (item.GetComponent<ShapingObjectSelector>() == null)
            {
                item.AddComponent<ShapingObjectSelector>();
            }
        }
    }
}
