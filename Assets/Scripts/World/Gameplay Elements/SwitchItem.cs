using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchItem : MonoBehaviour {

    //Should contain logic on on/off,
    // as well as which object it manipulates.
    //[HideInInspector]
    //public List<GameObject> assignedHazards;

    /*[Header("Hazard Objects:")]
    [Header("lock inspector window, select all, and drag & drop.")]
    public HazardState[] hazards;*/

    [Header("How long is the switch untouchable after collision?")]
    public float triggerDelay = 1f;
    
    [Header("Can the switch only be triggered once?")]
    public bool oneTimeTrigger = false;
    
    [Header("Choose on/off colours:")]
    public Color offColour = Color.red;
    public Color onColour = Color.green;

    [Header("Add the actual button part of the switch!")]
    public GameObject actualButtonPart;

    private bool countingDown = false;
    private bool hasBeenTriggered = false;
    private Renderer switchRenderer;
    private bool on = false;

    Room inRoom;

    void Start()
    {
        if (actualButtonPart != null)
            switchRenderer = actualButtonPart.GetComponent<Renderer>();
        else
            Debug.Log("Couldn't find button in switch prefab. Did you remember to drag & drop it?");
        SwitchColor();
    }

    public void AssignRoom(Room r)
    {
        inRoom = r;
        r.AddRoomSwitch(this);
    }

    public IEnumerator CountDown()
    {
        //wait the set time, then set bool to false
        yield return new WaitForSeconds(triggerDelay);
        countingDown = false;
    }

    //switch the color
    public void SwitchColor()
    {
        on = !on;
        if (actualButtonPart != null)
            switchRenderer.material.color = on ? onColour : offColour;
        else
            Debug.Log("Couldn't find button in switch prefab");
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
            //if we are allowed to trigger more than once || it hasn't been triggered yet:
            if (!oneTimeTrigger && !countingDown|| !hasBeenTriggered)
            {
                countingDown = true;
                //start counting down to next available switch:
                StartCoroutine(CountDown());

                hasBeenTriggered = true;

                var evt = new ObserverEvent(EventName.SwitchPressed);
                evt.payload.Add(PayloadConstants.SWITCH_ON, false);
                Subject.instance.Notify(gameObject, evt);

                inRoom.SwitchWasTouched();

               /* foreach (HazardState state in hazards)
                {
                    state.EnabledOrDisableTrap();

                    evt = new ObserverEvent(EventName.SwitchPressed);
                    evt.payload.Add(PayloadConstants.SWITCH_ON, false);
                    Subject.instance.Notify(gameObject, evt);
                }*/
            }
        }
    }
}
