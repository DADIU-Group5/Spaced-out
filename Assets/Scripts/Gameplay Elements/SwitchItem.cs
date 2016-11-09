using UnityEngine;
using System.Collections.Generic;

public class SwitchItem : MonoBehaviour {

    //Should contain logic on on/off,
    // as well as which object it manipulates.
   // [HideInInspector]
    public List<GameObject> assignedHazards;

    void Start()
    {
        foreach (GameObject hazard in assignedHazards)
        {
            hazard.GetComponent<HazardState>().hazardSwitch = this.gameObject;
        }
    }

    /// <summary>
    /// Assign an object to this switch
    /// </summary>
    public void AssignHazardToSwitch(GameObject hazard)
    {
        assignedHazards.Add(hazard);
    }

    /// <summary>
    ///if and object or the player taps the switch, turn off/on
    ///should I only consider objects thrown by player? How would I consider that? 
    ///if player bumps into an item, bool Thrown = true?
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "object")
        {
            Debug.Log("switch found player!");
            foreach (GameObject hazard in assignedHazards)
            {
                Debug.Log("hazards being turned off! item: " + hazard.name);
                hazard.GetComponent<HazardState>().EnabledOrDisableTrap();
            }
        }
    }
}
