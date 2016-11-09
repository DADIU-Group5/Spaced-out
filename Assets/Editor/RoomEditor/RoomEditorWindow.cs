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
                CreateBasicRoom();
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
            if (states == States.CreateRoom)
            {
                return;
            }
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
            if (Selection.activeGameObject.GetComponent<ShapingObjectSelector>() != null)
            {
                if (GUILayout.Button("Door"))
                {
                    CreateDoorInWall(Selection.activeGameObject);
                }
            }
            else
            {
                GUILayout.Space(21);
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Object selected");
            GUILayout.Space(85);
        }
        GUILayout.FlexibleSpace();
        thing = EditorGUILayout.Popup("Create new object: ", thing, new string[] {"Select", "Enviromental", "Floating", "Static", "Shaping", "Door" });
        switch (thing)
        {
            case 1:
                Selection.activeGameObject = RC.AddNewEnvironmentalObject();
                thing = 0;
                break;
            case 2:
                Selection.activeGameObject = RC.AddFloatingObject();
                thing = 0;
                break;
            case 3:
                Selection.activeGameObject = RC.AddStaticObject();
                thing = 0;
                break;
            case 4:
                Selection.activeGameObject = RC.AddNewshapingObject();
                thing = 0;
                break;
            case 5:
                Selection.activeGameObject = RC.AddNewDoor();
                thing = 0;
                break;
            default:
                break;
        }
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Create basic room"))
        {
            states = States.CreateRoom;
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

    int thing = 0;

    void Duplicate(GameObject toDuplicate)
    {
        GameObject go = Instantiate(toDuplicate,toDuplicate.transform.position,toDuplicate.transform.rotation,toDuplicate.transform.parent) as GameObject;
        go.name = toDuplicate.name;
        Selection.activeGameObject = go;
    }

    Vector3 size = new Vector3(5,5,5);

    void CreateBasicRoom()
    {
        GUILayout.Label("Creating a basic room");
        GUILayout.FlexibleSpace();
        size = EditorGUILayout.Vector3Field("Size: ", size);
        if (GUILayout.Button("Create Room (NYI)", GUILayout.Height(50)))
        {
            CreateRoomGeometry();
            states = States.Editing;
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Back", GUILayout.Height(50)))
        {
            states = States.Editing;
        }
        GUILayout.FlexibleSpace();
    }

    void CreateRoomGeometry()
    {
        //Floor
        GameObject floor = RC.AddNewshapingObject();
        floor.transform.localScale = new Vector3(size.x, size.z, 1);
        floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        floor.transform.position = new Vector3(0, -size.y / 2, 0);

        //Roof
        GameObject roof = RC.AddNewshapingObject();
        roof.transform.localScale = new Vector3(size.x, size.z, 1);
        roof.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        roof.transform.position = new Vector3(0,size.y/2,0);

        //Right wall
        GameObject right = RC.AddNewshapingObject();
        right.transform.localScale = new Vector3(size.z, size.y, 1);
        right.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
        right.transform.position = new Vector3(size.x/2, 0, 0);

        //Left wall
        GameObject left = RC.AddNewshapingObject();
        left.transform.localScale = new Vector3(size.z, size.y, 1);
        left.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
        left.transform.position = new Vector3(-size.x / 2, 0, 0);

        //Front wall
        GameObject front = RC.AddNewshapingObject();
        front.transform.localScale = new Vector3(size.x, size.y, 1);
        front.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        front.transform.position = new Vector3(0, 0, size.z / 2);

        //Back wall
        GameObject back = RC.AddNewshapingObject();
        back.transform.localScale = new Vector3(size.x, size.y, 1);
        back.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        back.transform.position = new Vector3(0, 0, -size.z / 2);
    }

    void CreateDoorInWall(GameObject wall)
    {
        if(wall.transform.localScale.x < 5 || wall.transform.localScale.y < 5)
        {
            Debug.LogError("Tried to create a door on a too small wall.");
            return;
        }
        //Door
        GameObject door = RC.AddNewDoor();
        door.transform.position = wall.transform.position;
        door.transform.rotation = Quaternion.FromToRotation(door.transform.right, wall.transform.forward);

        Vector3 tempSizes = wall.transform.localScale;
        Vector3 yScale = new Vector3(5, (tempSizes.y / 2) - 2.5f, 1);
        Vector3 xScale = new Vector3((tempSizes.x / 2) - 2.5f, tempSizes.y, 1);

        float xMove = 2.5f + (xScale.x / 2);
        float yMove = 2.5f + (yScale.y / 2);

        //Upper wall
        GameObject upper = RC.AddNewshapingObject();
        upper.transform.localScale = yScale;
        upper.transform.rotation = wall.transform.rotation;
        upper.transform.position = (wall.transform.up * yMove) + wall.transform.position;

        //Lower wall
        GameObject lower = RC.AddNewshapingObject();
        lower.transform.localScale = yScale;
        lower.transform.rotation = wall.transform.rotation;
        lower.transform.position = (wall.transform.up * -yMove) + wall.transform.position;

        //Right wall
        GameObject right = RC.AddNewshapingObject();
        right.transform.localScale = xScale;
        right.transform.rotation = wall.transform.rotation;
        right.transform.position = (wall.transform.right * xMove) + wall.transform.position;

        //Left wall
        GameObject left = RC.AddNewshapingObject();
        left.transform.localScale = xScale;
        left.transform.rotation = wall.transform.rotation;
        left.transform.position = (wall.transform.right * -xMove)+wall.transform.position;

        DestroyImmediate(wall);
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
