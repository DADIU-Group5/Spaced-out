using UnityEngine;
using System.Collections.Generic;

public class SwitchItem : MonoBehaviour {

    //Should contain logic on on/off,
    // as well as which object it manipulates.
   // [HideInInspector]
    public List<GameObject> assignedHazards;


    /// <summary>
    /// Assign an object to this switch
    /// </summary>
    public void AssignHazardToSwitch(GameObject hazard)
    {
        assignedHazards.Add(hazard);
    }

    //if and object or the player taps the switch, turn off/on
    //should I only consider objects thrown by player? How would I consider that? 
    //if player bumps into an item, bool Thrown = true?
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {
            foreach (GameObject hazard in assignedHazards)
            {
                hazard.GetComponent<HazardState>().isOn = !hazard.GetComponent<HazardState>().isOn;
            }
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
