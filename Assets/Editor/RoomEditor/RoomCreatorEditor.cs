using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    RoomCreator RC;

    enum States { noRoom, Editing, Saving, Loading }
    States states;

    string roomName;

    string pathName = "Assets/Resources/Rooms/";

    public override void OnInspectorGUI()
    {
        RC = (RoomCreator)target;
        //DrawDefaultInspector();
        if (rooms == null || rooms.Count == 0)
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
        }
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
        if (GUILayout.Button("Create Dynamic Object"))
        {
            RC.AddNewDynamicObject();
        }
        if (GUILayout.Button("Create Shaping Object"))
        {
            RC.AddNewshapingObject();
        }
        if (GUILayout.Button("Create Door"))
        {
            RC.AddNewDoor();
        }
        if (GUILayout.Button("Clear!"))
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

    int LoadID = 0;
    List<UnityEngine.Object> rooms;

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
        rooms = new List<UnityEngine.Object>(Resources.LoadAll("Rooms"));
    }
}
