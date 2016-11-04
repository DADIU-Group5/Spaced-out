using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour {

    [HideInInspector]
    public HazardState state;
    private Animator animator;
    private bool lastOnOff = true;

    public bool doorIsMalfunctioning = false;
    private bool malfunctioning = false;
    private bool closed = true;

    public void CloseOpenDoor()
    {
        if (closed)
        {
            closed = false;
            Debug.Log("opening doors");
            animator.SetTrigger("Open");
        } else
        {
            closed = true;
            Debug.Log("closing doors");
            animator.SetTrigger("Close");
        }
    }

	// Use this for initialization
	void Start () {
        state = gameObject.GetComponent<HazardState>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        
        //this only opens/closes if the isOn state is changed.
        //if the door isOn == true, then malfunctioning is set to false.
	    if (!state.isOn && closed)
        {
            CloseOpenDoor();
        } else if (state.isOn && !closed)
        {
            CloseOpenDoor();
        }
    }
}
