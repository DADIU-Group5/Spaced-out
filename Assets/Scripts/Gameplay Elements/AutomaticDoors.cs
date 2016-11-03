using UnityEngine;
using System.Collections;

public class AutomaticDoors : MonoBehaviour {

    [HideInInspector]
    public HazardState state;
    private Animator animator;
    private bool lastOnOff = true;

    public bool malfunctioning = true;
    private bool closed = true;

    public void CloseOpenDoor()
    {
        //yield return new WaitForSeconds(Random.Range[2, 4]);
        if (!state.isOn || malfunctioning && closed)
        {
            Debug.Log("opening doors");
            animator.SetTrigger("Open");
        } else
        {
            Debug.Log("closing doors");
            animator.SetTrigger("Close");
        }
    }

	// Use this for initialization
	void Start () {
        state = gameObject.GetComponent<HazardState>();
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(MalfunctioningDoors());
    }

    public IEnumerator MalfunctioningDoors()
    {
        while (malfunctioning)
        {
            Debug.Log("doors are malfunctioning");
            CloseOpenDoor();
            closed = !closed;
            yield return new WaitForSeconds(Random.Range(2, 4));
        }
    }
	
	// Update is called once per frame
	void Update () {

        //this only opens/closes if the isOn state is changed.
        //if the door isOn == true, then malfunctioning is set to false.
	    if (!state.isOn && state.isOn != lastOnOff)
        {
            malfunctioning = false;
            GetComponent<Collider>().enabled = true;
            CloseOpenDoor();
        } else if (state.isOn != lastOnOff)
        {
            GetComponent<Collider>().enabled = false;
            CloseOpenDoor();
        }
        lastOnOff = state.isOn;

    }
}
