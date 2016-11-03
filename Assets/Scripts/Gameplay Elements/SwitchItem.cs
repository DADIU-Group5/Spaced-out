using UnityEngine;
using System.Collections;

public class SwitchItem : MonoBehaviour {

    //Should contain logic on on/off,
    // as well as which object it manipulates.
   // [HideInInspector]
    public GameObject assignedHazard;

    public void AssignHazardToSwitch(GameObject hazard)
    {
        assignedHazard = hazard;
    }

    //if and object or the player taps the switch, turn off/on
    //should I only consider objects thrown by player? How would I consider that? 
    //if player bumps into an item, bool Thrown = true?
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {
            assignedHazard.GetComponent<HazardState>().isOn = !assignedHazard.GetComponent<HazardState>().isOn;
        }
    }

    /*//should it return when not touched?
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {   
            
        }
    }*/
}
