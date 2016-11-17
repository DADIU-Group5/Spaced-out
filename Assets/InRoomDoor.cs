using UnityEngine;
using System.Collections;

public class InRoomDoor : MonoBehaviour {

    public GameObject doorToLock;
    public Room prevRoom;
    public Room nextRoom;

	public void UnlockDoor()
    {
        doorToLock.SetActive(false);
    }

    public void LockDoor()
    {
        doorToLock.SetActive(true);
    }

    public void SetPrevRoom(Room room)
    {
        prevRoom = room;
    }

    public void SetNextRoom(Room room)
    {
        nextRoom = room;
    }

    public void EnteredNextRoom()
    {
        LockDoor();
        prevRoom.LeftThisRoom();
        nextRoom.EnteredThisRoom();
    }

    public void PrepNextRoom()
    {
        nextRoom.PrepRoom();
        UnlockDoor();
    }
}
