using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    List<Object> rooms = new List<Object>();
    List<Object> availableRooms = new List<Object>();
    List<Room> spawnedRooms = new List<Room>();

    Object lastSpawnedRoom = null;

    public GameObject doorPrefab;
    public GameObject lastDoor;
    public int exteriorSeed;
    public int interiorSeed;
    public int minRooms = 7;
    public int maxRooms = 20;
    public float distanceBetweenRooms = 3;

    Door firstDoor = null;

    List<Bounds> allBounds = new List<Bounds>();
    
	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
        if (exteriorSeed == -1)
        {
            exteriorSeed = Random.Range(0, 100);
        }

        Random.InitState(exteriorSeed);
        LoadRooms();
        int roomsToCreate = Random.Range(minRooms, maxRooms);
        Debug.Log("Length: " + roomsToCreate);
        CreateLevel(roomsToCreate);
        Debug.Log("Created: " + spawnedRooms.Count);
        RemoveUnusedDoors();
        RandomizeInteriorForAll();
	}

    void RandomizeInteriorForAll()
    {
        Random.InitState(interiorSeed);
        foreach (Room item in spawnedRooms)
        {
            item.RandomizeInterior();
        }
    }

    /// <summary>
    /// Creates the level.
    /// </summary>
    void CreateLevel(int lenght)
    {
        int createdRooms = 0;
        int tries = 100000;
        while (createdRooms < lenght)
        {
            if (CreateRoom())
            {
                createdRooms++;
            }
            else
            {
                createdRooms--;
                Destroy(spawnedRooms[spawnedRooms.Count - 1].gameObject);
                spawnedRooms.RemoveAt(spawnedRooms.Count - 1);
                allBounds.RemoveAt(spawnedRooms.Count - 1);
                lastDoor = GetRandomDoor(spawnedRooms[spawnedRooms.Count - 1]);
            }
            tries--;
            if(tries == 0)
            {
                Debug.Log("tried more than 100000 times");
                break;
            }
        }
    }

    bool CreateRoom()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CreateNewRoom())
            {
                return true;
            }
        }
        return false;
    }

    bool CreateNewRoom()
    {
        //Creates a new room. Get a reference to the room script, and get a random entrance door.
        GameObject newRoom = Instantiate(GetavailableRoom()) as GameObject;
        Room theRoom = newRoom.GetComponent<Room>();
        GameObject entranceDoor = GetRandomDoor(theRoom);
        if (firstDoor == null)
        {
            firstDoor = entranceDoor.GetComponent<Door>();
        }

        //Gets the rotation of the room. Should be rotated equal to the difference between the last and new doors right axis.
        Quaternion rot = Quaternion.FromToRotation(entranceDoor.transform.right, -lastDoor.transform.right);
        newRoom.transform.rotation = rot;

        //Moves the new room into position.
        newRoom.transform.position = newRoom.transform.position + (lastDoor.transform.position - entranceDoor.transform.position) - (entranceDoor.transform.right * distanceBetweenRooms);

        //Get bounds.
        Bounds newBound = newRoom.AddComponent<calcbounds>().calc();

        if (DoesRoomIntersect(newBound))
        {
            Destroy(newRoom);
            return false;
        }
        entranceDoor.GetComponent<Door>().ConnectRoom(lastDoor);
        if (lastDoor != null)
        {
            lastDoor.GetComponent<Door>().SetExit();
        }

        //Make a random door the exit.
        lastDoor = GetRandomDoor(theRoom);
        if (lastDoor == entranceDoor)
        {
            Debug.LogError("Something went wrong, you need to 'connect' a door, when using it.");
        }
        spawnedRooms.Add(theRoom);
        lastSpawnedRoom = (Object)newRoom;
        allBounds.Add(newBound);
        //theRoom.RandomizeInterior();
        return true;
    }

    /// <summary>
    /// Removes unused doors, places a 'real' door between connections.
    /// </summary>
    void RemoveUnusedDoors()
    {
        firstDoor.BreakConnection();
        foreach (Room item in spawnedRooms)
        {
            foreach (GameObject door in item.doorObjects)
            {
                if (door.GetComponent<Door>().GetDoorType() == DoorType.entrance)
                {
                    GameObject newDoor = Instantiate(doorPrefab);
                    newDoor.transform.rotation = door.transform.rotation;
                    newDoor.transform.position = door.transform.position + (door.transform.right * (distanceBetweenRooms / 2));
                    newDoor.transform.parent = door.transform.parent;
                    Destroy(door);
                }
                else
                {
                    door.GetComponent<Door>().CheckConnection();
                }
            }
        }
    }
	
    /// <summary>
    /// Load all the rooms from the database.
    /// </summary>
	void LoadRooms()
    {
        rooms = new List<Object>(Resources.LoadAll("Rooms"));
        availableRooms = new List<Object>(rooms);
    }

    /// <summary>
    /// Creates a random room.
    /// </summary>
    bool CreateRandomRoom()
    {
        //Creates a new room. Get a reference to the room script, and get a random entrance door.
        GameObject newRoom = Instantiate(GetavailableRoom()) as GameObject;
        Room theRoom = newRoom.GetComponent<Room>();
        GameObject entranceDoor = GetRandomDoor(theRoom);
        if(firstDoor == null)
        {
            firstDoor = entranceDoor.GetComponent<Door>();
        }

        //Gets the rotation of the room. Should be rotated equal to the difference between the last and new doors right axis.
        Quaternion rot = Quaternion.FromToRotation(entranceDoor.transform.right, -lastDoor.transform.right);
        newRoom.transform.rotation = rot;

        //Moves the new room into position.
        newRoom.transform.position = newRoom.transform.position + (lastDoor.transform.position - entranceDoor.transform.position)-(entranceDoor.transform.right* distanceBetweenRooms);

        //Get bounds.
        Bounds newBound = newRoom.AddComponent<calcbounds>().calc();
        
        if (DoesRoomIntersect(newBound))
        {
            Destroy(newRoom);
            return false;
        }
        else
        {
            entranceDoor.GetComponent<Door>().ConnectRoom(lastDoor);
            if (lastDoor != null)
            {
                lastDoor.GetComponent<Door>().ConnectRoom(newRoom);
            }

            //Make a random door the exit.
            lastDoor = GetRandomDoor(theRoom);
            if (lastDoor == entranceDoor)
            {
                Debug.LogError("Something went wrong, you need to 'connect' a door, when using it.");
            }
        }
        spawnedRooms.Add(theRoom);
        lastSpawnedRoom = (Object)newRoom;
        allBounds.Add(newBound);
        theRoom.RandomizeInterior();
        return true;
    }

    bool DoesRoomIntersect(Bounds newB)
    {
        foreach (Bounds item in allBounds)
        {
            if (item.Intersects(newB))
            {
               // Debug.Log("Early exit, intersection!");
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Gets a random unused door.
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    GameObject GetRandomDoor(Room r)
    {
        //SHOULD PROBABLY BE REDONE.
        //There is a SMALL chance that it will fail, even though there is a possible door. 
        //but, because there should only be 1 already used door, and there has to be atleast 2 doors.
        //there should be atleast a 50% chance to get the correct door each time. 
        //However it is not very optimized.

        //The amount of doors.
        int amount = r.doorObjects.Count;
        int rand;
        //Amount of tries.
        int counter = 100;
        while (true)
        {
            counter--;
            rand = Random.Range(0, amount);
            //If not already used, use it.
            if (!r.doorObjects[rand].GetComponent<Door>().Connected())
            {
                return r.doorObjects[rand];
            }
            if (counter == 0)
            {
                break;
            }
        }
        Debug.LogError("no valid door, should not happen!");
        return null;
    }

    GameObject GetavailableRoom()
    {
        if (availableRooms.Count == 0)
        {
            availableRooms = new List<Object>(rooms);
        }
        int rand = Random.Range(0, availableRooms.Count);
        GameObject go = (GameObject)availableRooms[rand];
        availableRooms.RemoveAt(rand);

        if(lastSpawnedRoom == go)
        {
            return GetavailableRoom();
        }

        return go;
    }
}
