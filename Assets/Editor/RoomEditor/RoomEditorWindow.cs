using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RoomEditorWindow : EditorWindow {
    
    public static RoomEditorWindow window;
    RoomCreator RC;

    enum States { noRoom, Editing, Saving, Loading, CreateRoom }
    States states;

    string roomName;

    List<Object> rooms;
    int loadID = 0;

    int objectToCreate = 0;

    string pathName = "Assets/Resources/Rooms/";

    [MenuItem("RoomEditor/RoomWindow")]
    public static void ShowWindow()
    {
        window = (RoomEditorWindow)EditorWindow.GetWindow(typeof(RoomEditorWindow));
        window.titleContent = new GUIContent("Room Editor", "Used to create and edit rooms.");
    }

    void OnGUI()
    {
        Selection.selectionChanged += RepaintThis;
        if (RC == null)
        {
            NoRoomCreator();
        }
        if (RC != null)
        {
            GetState();
        }
        switch (states)
        {
            case States.noRoom:
                NoRoom();
                break;
            case States.Editing:
                Editing();
                break;
            case States.Saving:
                SavingRoom();
                break;
            case States.Loading:
                LoadingRooms();
                break;
            case States.CreateRoom:
               // CreateBasicRoom();
                break;
            default:
                break;
        }
    }

    void RepaintThis()
    {
        Repaint();
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
            /*if (states == States.CreateRoom)
            {
                return;
            }*/
            states = States.Editing;
        }
    }

    void NoRoom()
    {
        GUILayout.Label("Currently no room, create a new room or load an existing room.");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("New room", GUILayout.Height(50)))
        {
            NewRoom();
        }
        GUILayout.FlexibleSpace();
            
        if (GUILayout.Button("Load room", GUILayout.Height(50)))
        {
            states = States.Loading;
        }
        GUILayout.FlexibleSpace();
    }

    void Editing()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.GetComponent<ObjectSelector>() != null)
            {
                EditorGUILayout.LabelField("Currently selected object: ", Selection.activeGameObject.name);
                if (GUILayout.Button("Duplicate"))
                {
                    Duplicate(Selection.activeGameObject);
                }
                if (GUILayout.Button("Delete"))
                {
                    DestroyImmediate(Selection.activeGameObject);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No duplicatable object selected");
                GUILayout.Space(42);
            }
            if (GUILayout.Button("Focus"))
            {
                SceneView.lastActiveSceneView.FrameSelected();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Object selected");
            GUILayout.Space(64);
        }
        GUILayout.FlexibleSpace();
        objectToCreate = EditorGUILayout.Popup("Create new object: ", objectToCreate, new string[] {"Select", "Enviromental", "Floating", "Static", "Shaping", "Pickup", "Door"});
        switch (objectToCreate)
        {
            case 1:
                Selection.activeGameObject = RC.AddNewEnvironmentalObject();
                objectToCreate = 0;
                break;
            case 2:
                Selection.activeGameObject = RC.AddFloatingObject();
                objectToCreate = 0;
                break;
            case 3:
                Selection.activeGameObject = RC.AddStaticObject();
                objectToCreate = 0;
                break;
            case 4:
                Selection.activeGameObject = RC.AddNewshapingObject();
                objectToCreate = 0;
                break;
            case 5:
                Selection.activeGameObject = RC.AddNewPickupObject();
                objectToCreate = 0;
                break;
            case 6:
                Selection.activeGameObject = RC.AddNewDoor();
                objectToCreate = 0;
                break;
            default:
                break;
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear!", GUILayout.Height(50)) && EditorUtility.DisplayDialog("Clear?", "Are you sure you want to clear the current room? Will erase any unsaved changes!", "I am sure", "Cancel"))
        {
            RC.DestroyRoom();
        }
        if (GUILayout.Button("Save!", GUILayout.Height(50)))
        {
            RC.GetRoom().CleanData();
            UpdateAll();
            roomName = RC.GetName();
            if (RC.GetRoom().canBeRoom())
            {
                states = States.Saving;
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    void Duplicate(GameObject toDuplicate)
    {
        GameObject go = Instantiate(toDuplicate,toDuplicate.transform.position,toDuplicate.transform.rotation,toDuplicate.transform.parent) as GameObject;
        go.name = toDuplicate.name;
        Selection.activeGameObject = go;
    }

    void SavingRoom()
    {
        if(roomName == null)
        {
            roomName = RC.GetName();
        }
        roomName = EditorGUILayout.TextField("Room Name", roomName);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Temporary save!", GUILayout.Height(50)))
        {
            if (SaveObject())
            {
                states = States.Editing;
            }
        }
        if (GUILayout.Button("Save and finalize!", GUILayout.Height(50)))
        {
            if (SaveObject())
            {
                RC.DestroyRoom();
            }
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            states = States.Editing;
        }
        GUILayout.FlexibleSpace();
    }

    bool SaveObject()
    {
        if (roomName == "")
        {
            EditorUtility.DisplayDialog("Invalid name", "The name: \"" + roomName + "\" is not allowed!", "Back");
            return false;
        }
        if(roomName.Length < 3)
        {
            EditorUtility.DisplayDialog("Invalid name", "The name: \"" + roomName + "\" is too short", "Back");
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
            GetRoomsFromDatabase();
            return true;
        }
    }

    bool AssetWithNameExists(string _name)
    {
        if (rooms == null)
        {
            return false;
        }
        foreach (Object item in rooms)
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

    void NewRoom()
    {
        RC.CreateNewRoom();
    }

    void LoadingRooms()
    {
        GetRoomsFromDatabase();
        EditorGUILayout.LabelField("Number of rooms: ", rooms.Count.ToString());
        if(rooms.Count == 0)
        {
            GUILayout.Label("There are no rooms in the database!");
            return;
        }
        EditorGUILayout.LabelField("Previewing: ", (loadID + 1).ToString());
        EditorGUILayout.LabelField("Name: ", rooms[loadID].name);
        GUILayout.Label(AssetPreview.GetAssetPreview(rooms[loadID]),EditorStyles.centeredGreyMiniLabel);

        if (GUILayout.Button("Load"))
        {
            RC.LoadRoom((GameObject)rooms[loadID]);
            roomName = rooms[loadID].name;
        }
        if (GUILayout.Button("Remove") && EditorUtility.DisplayDialog("DELETE?", "Are you sure you want to delete this room? CANNOT be undone!", "YES!", "Cancel"))
        {
            FileUtil.DeleteFileOrDirectory(pathName + rooms[loadID].name + ".prefab");
            EditorUtility.UnloadUnusedAssetsImmediate();
            AssetDatabase.Refresh();
            GetRoomsFromDatabase();
            if (loadID >= rooms.Count)
            {
                loadID = rooms.Count - 1;
            }
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Prev"))
        {
            loadID--;
            if (loadID < 0)
            {
                loadID = rooms.Count - 1;
            }
        }
        if (GUILayout.Button("Next"))
        {
            loadID++;
            if (loadID >= rooms.Count)
            {
                loadID = 0;
            }
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Back"))
        {
            states = States.noRoom;
        }
    }

    void GetRoomsFromDatabase()
    {
        rooms = new List<Object>(Resources.LoadAll("Rooms"));
    }

    void NoRoomCreator()
    {
        RC = FindObjectOfType<RoomCreator>();
        if (RC == null)
        {
            Debug.LogError("There are no Room Creator in the scene, you should use the Room Editor scene!");
        }
    }

    void AddRC()
    {
        if(Selection.activeGameObject.GetComponent<RoomCreator>() != null)
        {
            RC = Selection.activeGameObject.GetComponent<RoomCreator>();
        }
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
