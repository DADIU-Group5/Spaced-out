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
    public int seed;
    public int minRooms = 7;
    public int maxRooms = 20;

    Door firstDoor = null;

    List<Bounds> allBounds = new List<Bounds>();
    
	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
        if (seed == -1)
        {
            seed = Random.Range(0, 100);
        }

        Random.InitState(seed);
        LoadRooms();

        for (int i = 0; i < Random.Range(minRooms, maxRooms); i++)
        {
            if (!CreateRandomRoom())
            {
                break;
            }
        }

        firstDoor.BreakConnection();
        foreach (Room item in spawnedRooms)
        {
            foreach (GameObject door in item.doorObjects)
            {
                door.GetComponent<Door>().CheckConnection();
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
        newRoom.transform.position = newRoom.transform.position + (lastDoor.transform.position - entranceDoor.transform.position)-entranceDoor.transform.right;

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
                Debug.Log("Early exit, intersection!");
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
