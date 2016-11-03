using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    List<Object> rooms = new List<Object>();
    public GameObject lastDoor;
    public int seed;
    public int minRooms = 7;
    public int maxRooms = 20;
    
	// Use this for initialization
	void Start () {
        seed = Random.Range(0, 100);
        Random.InitState(seed);
        LoadRooms();
        for (int i = 0; i < Random.Range(minRooms, maxRooms); i++)
        {
            CreateRandomRoom();
        }
	}
	
    /// <summary>
    /// Load all the rooms from the database.
    /// </summary>
	void LoadRooms()
    {
        rooms = new List<Object>(Resources.LoadAll("Rooms"));
    }

    /// <summary>
    /// Creates a random room.
    /// </summary>
    void CreateRandomRoom()
    {
        //Creates a new room. Get a reference to the room script, and get a random entrance door.
        GameObject newRoom = Instantiate((GameObject)rooms[Random.Range(0, rooms.Count)]) as GameObject;
        Room theRoom = newRoom.GetComponent<Room>();
        GameObject entranceDoor = GetRandomDoor(theRoom);

        //Gets the rotation of the room. Should be rotated equal to the difference between the last and new doors right axis.
        Quaternion rot = Quaternion.FromToRotation(entranceDoor.transform.right, -lastDoor.transform.right);
        newRoom.transform.rotation = rot;

        //Moves the new room into position.
        newRoom.transform.position = newRoom.transform.position + (lastDoor.transform.position - entranceDoor.transform.position);

        entranceDoor.GetComponent<Door>().ConnectRoom(lastDoor);
        
        //Make a random door the exit.
        lastDoor = GetRandomDoor(theRoom);
        if(lastDoor == entranceDoor)
        {
            Debug.LogError("Something went wrong, you need to 'connect' a door, when using it.");
        }
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
        int amount = r.doorObject.Count;
        int rand;
        //Amount of tries.
        int counter = 100;
        while (true)
        {
            counter--;
            rand = Random.Range(0, amount);
            //If not already used, use it.
            if (!r.doorObject[rand].GetComponent<Door>().Connected())
            {
                return r.doorObject[rand];
            }
            if (counter == 0)
            {
                break;
            }
        }
        Debug.LogError("no valid door, should not happen!");
        return null;
    }
}
