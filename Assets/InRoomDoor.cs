using UnityEngine;
using System.Collections;

public class InRoomDoor : MonoBehaviour {

    public MalfunctioningDoors doorToLock;
    public Room prevRoom;
    public Room nextRoom;

	public void UnlockDoor()
    {

    }

    public void LockDoor()
    {
        doorToLock.LockDoor();
    }

    public void SetPrevRoom(Room room)
    {
        prevRoom = room;
    }

    public void SetNextRoom(Room room)
    {
        nextRoom = room;
    }

    public void DoorLocked()
    {
        prevRoom.LeftThisRoom();
    }

    public void EnteredNextRoom()
    {
        LockDoor();
        if (nextRoom != null)
        {
            nextRoom.EnteredThisRoom();
            if (nextRoom.exitDoor != null)
            {
                nextRoom.exitDoor.doorToLock.StartDoor();
            }
        }
    }

    public void PrepNextRoom()
    {
        nextRoom.PrepRoom();
        UnlockDoor();
    }
}
