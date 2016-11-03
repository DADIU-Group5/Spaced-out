using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour {

    GameObject doorCheck0;
    GameObject doorCheck1;

    public HazardState state;
    //if the player is touching both of these checks, then player dies.

    //if door is closed,
    //  check both doorChecks
    //  if both are touching player
    //      kill player

    public void CheckForPlayer()
    {

    }

    public void CloseOpenDoor()
    {
        if (gameObject.GetComponent<Collider>().enabled)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.black;
        } else
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

	// Use this for initialization
	void Start () {
        state = gameObject.GetComponent<HazardState>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (state.isOn)
        {
            GetComponent<Collider>().enabled = true;
            CloseOpenDoor();
        } else
        {
            GetComponent<Collider>().enabled = false;
            CloseOpenDoor();
        }
	}
}
