using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RoomMakerEditor : EditorWindow {

    public static RoomMakerEditor window;
    RoomMaker RM;

    enum States { noRoom, Editing, Saving, Loading, CreateRoom, Props }
    States states;

    string roomName;

    List<Object> rooms;
    int loadID = 0;

    string pathName = "Assets/Resources/Rooms/";

    int objectToCreate = 0;

    Vector3 tempPos = Vector3.zero;

    [MenuItem("RoomEditor/RoomMakerWindow")]
    public static void ShowWindow()
    {
        window = (RoomMakerEditor)EditorWindow.GetWindow(typeof(RoomMakerEditor));
        window.titleContent = new GUIContent("Room Editor", "Used to create and edit rooms.");
    }

    void OnGUI()
    {
        Selection.selectionChanged += RepaintThis;
        if(RM == null)
        {
            NoRoomMaker();
        }
        if (RM != null)
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
                CreateBasicRoom();
                break;
            case States.Props:
                PropCreation();
                break;
            default:
                break;
        }
    }

    void GetState()
    {
        if (!RM.EditingRoom())
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
            if (states == States.CreateRoom)
            {
                return;
            }
            if(states == States.Props)
            {
                return;
            }
            states = States.Editing;
        }
    }

    void NoRoomMaker()
    {
        RM = FindObjectOfType<RoomMaker>();
        if (RM == null)
        {
            Debug.LogError("There are no Room Creator in the scene, you should use the Room Editor scene!");
        }
    }

    void NoRoom()
    {
        GUILayout.Label("Currently no room, create a new room or load an existing room.");
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New room", GUILayout.Height(50)))
        {
            RM.RegularRoom();
            NewRoom();
        }
        /*if (GUILayout.Button("New tall room", GUILayout.Height(50)))
        {
            RM.TallRoom();
            NewRoom();
        }*/
        EditorGUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Load room", GUILayout.Height(50)))
        {
            states = States.Loading;
        }
        GUILayout.FlexibleSpace();
    }

    void NewRoom()
    {
        RM.CreateNewRoom();
    }

    void Editing()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.activeInHierarchy)
        {
            if(Selection.activeGameObject.GetComponent<ObjectSelector>() == null)
            {
                if (Selection.activeTransform.parent != null)
                {
                    if (Selection.activeTransform.parent.GetComponent<ObjectSelector>() != null)
                    {
                        Selection.activeTransform = Selection.activeTransform.parent;
                    }
                }
            }
            if (Selection.activeGameObject.GetComponent<ObjectSelector>() != null)
            {
                EditorGUILayout.LabelField("Currently selected object: ", Selection.activeGameObject.name);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Duplicate UP"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(0, 4));
                }
                if (GUILayout.Button("Duplicate DOWN"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(0, -4));
                }
                if (GUILayout.Button("Duplicate RIGHT"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(4, 0));
                }
                if (GUILayout.Button("Duplicate LEFT"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(-4, 0)); ;
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Delete"))
                {
                    DestroyImmediate(Selection.activeGameObject);
                    return;
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
            if(Selection.activeGameObject.GetComponent<FloorSelector>() != null)
            {
                EditorGUILayout.LabelField("Floor selected");
                if (GUILayout.Button("Wall"))
                {
                    Selection.activeGameObject = RM.NewWall(Selection.activeTransform.position);
                }
                if (GUILayout.Button("OuterCornor"))
                {
                    Selection.activeGameObject = RM.NewOuter(Selection.activeTransform.position);
                }
                if (GUILayout.Button("Door"))
                {
                    Selection.activeGameObject = RM.NewDoor(Selection.activeTransform.position);
                }
                if (GUILayout.Button("REPLACE with inner cornor"))
                {
                    GameObject temp = RM.NewInner(Selection.activeTransform.position);
                    DestroyImmediate(Selection.activeGameObject);
                    Selection.activeGameObject = temp;
                }
            }
            if (Selection.activeGameObject.GetComponent<FloorSelector>() != null || Selection.activeGameObject.GetComponent<WallSelector>() != null || Selection.activeGameObject.GetComponent<OuterCornorSelector>() != null || Selection.activeGameObject.GetComponent<InnerCornorSelector>() != null || Selection.activeGameObject.GetComponent<Door>() != null)
            {
                EditorGUILayout.LabelField("Rotable Object Selected.");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+90"))
                {
                    Selection.activeTransform.Rotate(new Vector3(0, 90, 0));
                }
                if (GUILayout.Button("-90"))
                {
                    Selection.activeTransform.Rotate(new Vector3(0, -90, 0));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Object selected");
            GUILayout.Space(64);
        }
        GUILayout.FlexibleSpace();
        GUILayout.FlexibleSpace();
        objectToCreate = EditorGUILayout.Popup("Create new object: ", objectToCreate, new string[] { "Select", "Floor", "Wall", "InnerCornor", "OuterCornor", "Door" });
        if(Selection.activeTransform != null)
        {
            tempPos = Selection.activeTransform.position;
        }
        else
        {
            tempPos = Vector3.zero;
        }
        switch (objectToCreate)
        {
            case 1:
                Selection.activeGameObject = RM.NewFloor(tempPos);
                objectToCreate = 0;
                break;
            case 2:
                Selection.activeGameObject = RM.NewWall(tempPos);
                objectToCreate = 0;
                break;
            case 3:
                Selection.activeGameObject = RM.NewInner(tempPos);
                objectToCreate = 0;
                break;
            case 4:
                Selection.activeGameObject = RM.NewOuter(tempPos);
                objectToCreate = 0;
                break;
            case 5:
                Selection.activeGameObject = RM.NewDoor(tempPos);
                objectToCreate = 0;
                break;
            default:
                break;
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear!", GUILayout.Height(50)) && EditorUtility.DisplayDialog("Clear?", "Are you sure you want to clear the current room? Will erase any unsaved changes!", "I am sure", "Cancel"))
        {
            RM.DestroyRoom();
        }
        if (GUILayout.Button("Save!", GUILayout.Height(50)))
        {
            RM.GetRoom().CleanData();
            RM.GetRoom().UpdateLists();
            roomName = RM.GetName();
            if (RM.GetRoom().canBeRoom())
            {
                states = States.Saving;
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Set Props", GUILayout.Height(50)))
        {
            states = States.Props;
        }
    }

    void PropCreation()
    {
        if (Selection.activeGameObject != null && Selection.activeGameObject.activeInHierarchy)
        {
            if (Selection.activeGameObject.GetComponent<ObjectSelector>() == null)
            {
                if (Selection.activeTransform.parent != null)
                {
                    if (Selection.activeTransform.parent.GetComponent<ObjectSelector>() != null)
                    {
                        Selection.activeTransform = Selection.activeTransform.parent;
                    }
                }
            }
            if (Selection.activeGameObject.GetComponent<ObjectSelector>() != null)
            {
                EditorGUILayout.LabelField("Currently selected object: ", Selection.activeGameObject.name);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Duplicate UP"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(0, 4));
                }
                if (GUILayout.Button("Duplicate DOWN"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(0, -4));
                }
                if (GUILayout.Button("Duplicate RIGHT"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(4, 0));
                }
                if (GUILayout.Button("Duplicate LEFT"))
                {
                    Duplicate(Selection.activeGameObject, new Vector2(-4, 0)); ;
                }
                EditorGUILayout.EndHorizontal();
                if (GUILayout.Button("Delete"))
                {
                    DestroyImmediate(Selection.activeGameObject);
                    return;
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
            if (Selection.activeGameObject.GetComponent<SmallObjectSelector>() != null || Selection.activeGameObject.GetComponent<MediumObjectSelector>() != null || Selection.activeGameObject.GetComponent<LargeObjectSelector>() != null || Selection.activeGameObject.GetComponent<XLargeObjectSelector>() != null || Selection.activeGameObject.GetComponent<PickupSelector>() != null)
            {
                EditorGUILayout.LabelField("Rotable Object Selected.");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+90"))
                {
                    Selection.activeTransform.Rotate(new Vector3(0, 90, 0));
                }
                if (GUILayout.Button("-90"))
                {
                    Selection.activeTransform.Rotate(new Vector3(0, -90, 0));
                }
                EditorGUILayout.EndHorizontal();
            }
            SpawnHazardOrSwitch();
        }
        else
        {
            EditorGUILayout.LabelField("No Object selected");
            GUILayout.Space(64);
        }
        GUILayout.FlexibleSpace();


        GUILayout.FlexibleSpace();
        objectToCreate = EditorGUILayout.Popup("Create new object: ", objectToCreate, new string[] { "Select", "Small Object", "Medium Object", "Large Object", "XLarge Object", "Props", "Fuel", "Comic" });
        if (Selection.activeTransform != null)
        {
            tempPos = Selection.activeTransform.position;
        }
        else
        {
            tempPos = Vector3.zero;
        }
        switch (objectToCreate+2)
        {
            /*case 1:
                Selection.activeGameObject = RM.NewHazard(tempPos);
                objectToCreate = 0;
                break;
            case 2:
                Selection.activeGameObject = RM.NewSwitch(tempPos);
                objectToCreate = 0;
                break;*/
            case 3:
                Selection.activeGameObject = RM.NewProp(tempPos, 0);
                objectToCreate = 0;
                break;
            case 4:
                Selection.activeGameObject = RM.NewProp(tempPos, 1);
                objectToCreate = 0;
                break;
            case 5:
                Selection.activeGameObject = RM.NewProp(tempPos, 2);
                objectToCreate = 0;
                break;
            case 6:
                Selection.activeGameObject = RM.NewProp(tempPos, 3);
                objectToCreate = 0;
                break;
            case 7:
                Selection.activeGameObject = RM.NewFloatingProp(tempPos);
                objectToCreate = 0;
                break;
            case 8:
                Selection.activeGameObject = RM.NewPickUp(tempPos,0);
                objectToCreate = 0;
                break;
            case 9:
                Selection.activeGameObject = RM.NewPickUp(tempPos,1);
                objectToCreate = 0;
                break;
            default:
                break;
        }

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Edit room", GUILayout.Height(50)))
        {
            states = States.Editing;
        }
    }

    void SpawnHazardOrSwitch()
    {
        if (Selection.activeGameObject.GetComponent<WallSelector>() != null)
        {
            EditorGUILayout.LabelField("Hazard/Switch spot");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn Hazard here"))
            {
                RM.NewHazard(Selection.activeTransform.GetChild(0).position + new Vector3(0, 2, 0) + Selection.activeTransform.right * 0.5f, Selection.activeTransform.rotation);
            }
            if (GUILayout.Button("Spawn Switch here"))
            {
                RM.NewSwitch(Selection.activeTransform.GetChild(0).position + new Vector3(0, 2, 0) + Selection.activeTransform.right * 0.5f, Selection.activeTransform.rotation);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (Selection.activeGameObject.GetComponent<InnerCornorSelector>() != null)
        {
            EditorGUILayout.LabelField("Hazard/Switch spot");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn Hazard Left"))
            {
                RM.NewHazard(Selection.activeTransform.GetChild(1).position + Selection.activeTransform.GetChild(1).right * 0.5f, Selection.activeTransform.GetChild(1).rotation);
            }
            if (GUILayout.Button("Spawn Hazard Right"))
            {
                RM.NewHazard(Selection.activeTransform.GetChild(2).position + Selection.activeTransform.GetChild(2).right * 0.5f, Selection.activeTransform.GetChild(2).rotation);
            }
            if (GUILayout.Button("Spawn Switch Left"))
            {
                RM.NewSwitch(Selection.activeTransform.GetChild(1).position + Selection.activeTransform.GetChild(1).right * 0.5f, Selection.activeTransform.GetChild(1).rotation);
            }
            if (GUILayout.Button("Spawn Switch Right"))
            {
                RM.NewSwitch(Selection.activeTransform.GetChild(2).position + Selection.activeTransform.GetChild(2).right * 0.5f, Selection.activeTransform.GetChild(2).rotation);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (Selection.activeGameObject.GetComponent<OuterCornorSelector>() != null)
        {
            EditorGUILayout.LabelField("Hazard/Switch spot");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Spawn Hazard Left"))
            {
                RM.NewHazard(Selection.activeTransform.GetChild(1).position + Selection.activeTransform.GetChild(1).right * 0.5f, Selection.activeTransform.GetChild(1).rotation);
            }
            if (GUILayout.Button("Spawn Hazard Right"))
            {
                RM.NewHazard(Selection.activeTransform.GetChild(2).position + Selection.activeTransform.GetChild(2).right * 0.5f, Selection.activeTransform.GetChild(2).rotation);
            }
            if (GUILayout.Button("Spawn Switch Left"))
            {
                RM.NewSwitch(Selection.activeTransform.GetChild(1).position + Selection.activeTransform.GetChild(1).right * 0.5f, Selection.activeTransform.GetChild(1).rotation);
            }
            if (GUILayout.Button("Spawn Switch Right"))
            {
                RM.NewSwitch(Selection.activeTransform.GetChild(2).position + Selection.activeTransform.GetChild(2).right * 0.5f, Selection.activeTransform.GetChild(2).rotation);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void Duplicate(GameObject toDuplicate, Vector2 offset)
    {
        GameObject go = Instantiate(toDuplicate, toDuplicate.transform.position+new Vector3(offset.x,0,offset.y), toDuplicate.transform.rotation, toDuplicate.transform.parent) as GameObject;
        go.name = toDuplicate.name;
        Selection.activeGameObject = go;
    }

    void SavingRoom()
    {
        if (roomName == null)
        {
            roomName = RM.GetName();
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
                RM.DestroyRoom();
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
        if (roomName.Length < 3)
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
            Room roomToSave = RM.GetRoom();
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

    void LoadingRooms()
    {
        GetRoomsFromDatabase();
        EditorGUILayout.LabelField("Number of rooms: ", rooms.Count.ToString());
        if (rooms.Count == 0)
        {
            GUILayout.Label("There are no rooms in the database!");
            if (GUILayout.Button("Back"))
            {
                states = States.noRoom;
            }
            return;
        }
        EditorGUILayout.LabelField("Previewing: ", (loadID + 1).ToString());
        EditorGUILayout.LabelField("Name: ", rooms[loadID].name);
        GUILayout.Label(AssetPreview.GetAssetPreview(rooms[loadID]), EditorStyles.centeredGreyMiniLabel);

        if (GUILayout.Button("Load"))
        {
            RM.LoadRoom((GameObject)rooms[loadID]);
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

    void CreateBasicRoom()
    {

    }

    void RepaintThis()
    {
        Repaint();
    }

    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();
        if (GUILayout.Button("Press Me"))
            Debug.Log("Got it to work.");

        Handles.EndGUI();
    }
}