using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour {

    [HideInInspector]
    public HazardState state;

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
