﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    List<Object> rooms = new List<Object>();
    List<Object> availableRooms = new List<Object>();
    List<Room> spawnedRooms = new List<Room>();

    Object lastSpawnedRoom = null;

    public GameObject doorPrefab;
    public GameObject lastDoor;

    public GameObject playerPrefab;
    public GameObject keyPrefab;
    public GameObject keyDoorPrefab;
    public GameObject entryDoorPrefab;

    public int exteriorSeed;
    public int interiorSeed;
    public int minRooms = 7;
    public int maxRooms = 20;
    public float distanceBetweenRooms = 3;
    public float playerDistanceFromDoor = 1;
    public float keyDistanceFromDoor = 1;

    Transform playerSpawnPoint;
    EntryCutScene ECS;

    Door firstDoor = null;
    GameObject EntryHall;

    public List<Bounds> allBounds = new List<Bounds>();
    
	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        //UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
        GenerateLevel();
    }

    /// <summary>
    /// Generates the entire level.
    /// </summary>
    void GenerateLevel()
    {
        GetGenerationData();

        Random.InitState(exteriorSeed);

        LoadRooms();

        int roomsToCreate = Random.Range(minRooms, maxRooms);

        CreateLevel(roomsToCreate);
        //StartCoroutine(test(roomsToCreate));

        SpawnKey();
        RemoveUnusedDoors();
        RandomizeInteriorForAll();
        DisableRooms();
        spawnedRooms[0].EnteredThisRoom();
        spawnedRooms[0].exitDoor.doorToLock.StartDoor();
        foreach (Room item in spawnedRooms)
        {
            Destroy(item.GetComponent<CalcBounds>());
            item.SetStatic();
        }
        Destroy(EntryHall.GetComponent<CalcBounds>());
        SpawnPlayer();
    }

    void GetGenerationData()
    {
        var data = GenerationDataManager.instance.GetLevelData();
        exteriorSeed = data.exteriorSeed;
        interiorSeed = data.interiorSeed;
        minRooms = data.rooms;
        maxRooms = minRooms;
    }

    /// <summary>
    /// Randomizes all the objects in the rooms.
    /// </summary>
    void RandomizeInteriorForAll()
    {
        Random.InitState(interiorSeed);
        foreach (Room item in spawnedRooms)
        {
            item.RandomizeInterior();
        }
    }

    /// <summary>
    /// Spawns the player in front of the first door in the first room.
    /// </summary>
    void SpawnPlayer()
    {
        CheckpointManager.instance.SetSpawnDistance(playerDistanceFromDoor + 1);
        GameObject go = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
        go.transform.LookAt(transform.position /*+ new Vector3(0, 2, 0)*/, Vector3.up);
        go.GetComponentInChildren<OxygenController>().ReplenishOxygen();
        CheckpointManager.instance.SetFuelCount(go.GetComponentInChildren<OxygenController>().GetOxygen());

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, go.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
        evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(go);
    }

    /// <summary>
    /// Spawns the 'key' at a random doors position in the last room.
    /// </summary>
    void SpawnKey()
    {
        Transform tempTrans = GetRandomDoor(spawnedRooms[spawnedRooms.Count - 1]).transform;
        //Instantiate(keyPrefab, tempTrans.position - (tempTrans.right*-keyDistanceFromDoor) + new Vector3(0, 2, 0), Quaternion.identity,spawnedRooms[spawnedRooms.Count-1].transform);
        tempTrans.GetComponent<Door>().SetDoorBehindKey();
    }

    void DisableRooms()
    {
        for (int i = 2; i < spawnedRooms.Count; i++)
        {
            spawnedRooms[i].LeftThisRoom();
        }
    }

    /// <summary>
    /// Creates the level.
    /// </summary>
    void CreateLevel(int lenght)
    {
        int createdRooms = 0;
        int tries = 1000;
        while (createdRooms < lenght)
        {
            if (CreateRoom())
            {
                createdRooms++;
            }
            //If it could not create a room from a position, remove the previous room, and try again.
            //Should make sure it never hits a dead end.
            else
            {   
                createdRooms--;
                if (createdRooms > 0)
                {
                    Destroy(spawnedRooms[spawnedRooms.Count - 1].gameObject);
                    allBounds.RemoveAt(allBounds.Count - 1);
                    spawnedRooms.RemoveAt(spawnedRooms.Count - 1);
                    lastDoor = GetRandomDoor(spawnedRooms[spawnedRooms.Count - 1]);
                    foreach (GameObject item in spawnedRooms[spawnedRooms.Count - 1].doorObjects)
                    {
                        if (item.GetComponent<Door>().GetDoorType() == DoorType.exit)
                        {
                            item.GetComponent<Door>().BreakConnection();
                        }
                    }
                }
                else
                {
                    Debug.LogError("Something went wrong! This should NOT be possible. Talk to Frederik.");
                    break;
                }
            }
            //Makes sure that it does not end in an infinite loop, should not actually happen.
            tries--;
            if(tries == 0)
            {
                Debug.Log("tried more than 100000 times");
                break;
            }
        }
    }

    /*IEnumerator test(int lenght)
    {
        int createdRooms = 0;
        int tries = 1000;
        while (createdRooms < lenght)
        {
            yield return new WaitForSeconds(1);
            if (CreateRoom())
            {
                createdRooms++;
            }
            //If it could not create a room from a position, remove the previous room, and try again.
            //Should make sure it never hits a dead end.
            else
            {
                createdRooms--;
                if (createdRooms > 0)
                {
                    Destroy(spawnedRooms[spawnedRooms.Count - 1].gameObject);
                    allBounds.RemoveAt(allBounds.Count - 1);
                    spawnedRooms.RemoveAt(spawnedRooms.Count - 1);
                    lastDoor = GetRandomDoor(spawnedRooms[spawnedRooms.Count - 1]);
                    foreach (GameObject item in spawnedRooms[spawnedRooms.Count - 1].doorObjects)
                    {
                        if (item.GetComponent<Door>().GetDoorType() == DoorType.exit)
                        {
                            item.GetComponent<Door>().BreakConnection();
                        }
                    }
                }
                else
                {
                    Debug.LogError("Something went wrong! This should NOT be possible. Talk to Frederik.");
                    break;
                }
            }
            //Makes sure that it does not end in an infinite loop, should not actually happen.
            tries--;
            if (tries == 0)
            {
                Debug.Log("tried more than 100000 times");
                break;
            }
        }
    }*/

    /// <summary>
    /// Creates a new room, has 3 tries to do it, before it will say it cannot create it there.
    /// </summary>
    /// <returns></returns>
    bool CreateRoom()
    {
        for (int i = 0; i < 10; i++)
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

        //Gets the rotation of the room. Should be rotated equal to the difference between the last and new doors right axis.
        float lastY = lastDoor.transform.eulerAngles.y + 180;
        float newY = lastY - entranceDoor.transform.eulerAngles.y;
        newRoom.transform.Rotate(new Vector3(0, newY, 0));

        //Moves the new room into position.
        newRoom.transform.position = newRoom.transform.position + (lastDoor.transform.position - entranceDoor.transform.position) + (entranceDoor.transform.right * 7.3f);

        //Get bounds.
        Bounds newBound = newRoom.AddComponent<CalcBounds>().calc();

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
        if (firstDoor == null)
        {
            firstDoor = entranceDoor.GetComponent<Door>();
            firstDoor.SetEntry();
            EntryHall = Instantiate(entryDoorPrefab);
            EntryHall.transform.rotation = firstDoor.transform.rotation;
            EntryHall.transform.position = firstDoor.transform.position + (-firstDoor.transform.right * (5.65f));
            ECS = EntryHall.GetComponent<EntryCutScene>();
            playerSpawnPoint = ECS.GetPlayerSpawnPos();
            allBounds.Add(EntryHall.GetComponent<CalcBounds>().calc());
        }
        return true;
    }

    /// <summary>
    /// Removes unused doors, places a 'real' door between connections.
    /// </summary>
    void RemoveUnusedDoors()
    {
        int iterator = 0;

        GameObject newDoor;

        foreach (Room item in spawnedRooms)
        {
            foreach (GameObject door in item.doorObjects)
            {
                if (door.GetComponent<Door>().GetDoorType() == DoorType.entrance)
                {
                    newDoor = Instantiate(doorPrefab);
                    newDoor.transform.rotation = door.transform.rotation;
                    
                    newDoor.transform.position = door.transform.position + (-door.transform.right * (5.65f));

                    InRoomDoor IRD = newDoor.GetComponent<InRoomDoor>();
                    IRD.SetPrevRoom(spawnedRooms[iterator]);
                    IRD.SetNextRoom(spawnedRooms[iterator + 1]);
                    spawnedRooms[iterator].SetExitDoor(IRD);
                    iterator++;
                    Destroy(door);
                }
                else if (door.GetComponent<Door>().GetDoorType() == DoorType.Key)
                {
                    newDoor = Instantiate(keyDoorPrefab);
                    newDoor.transform.rotation = door.transform.rotation;

                    newDoor.transform.position = door.transform.position + (-door.transform.right * (5.65f));
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

    /// <summary>
    /// Returns a room to spawn next.
    /// </summary>
    /// <returns></returns>
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
