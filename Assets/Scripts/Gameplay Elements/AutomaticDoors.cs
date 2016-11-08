using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour {

    [HideInInspector]
    public HazardState state;
    private Animator animator;
    private bool lastOnOff = true;

    [HideInInspector]
    public int doorsTouchingPlayer = 0;

    private bool malfunctioning = false;
    private bool closed = true;
    private bool crushingPlayer = false;

    public void CloseOpenDoor()
    {
        if (closed)
        {
            closed = false;
            animator.SetTrigger("Open");
        } else
        {
            closed = true;
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

        if (doorsTouchingPlayer >= 2 && !crushingPlayer &&
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("DoorClose"))
        {
            crushingPlayer = true;
            var evt = new ObserverEvent(EventName.Crushed);
            Subject.instance.Notify(gameObject, evt);

            Debug.Log("Player has been crushed!");
        }

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
