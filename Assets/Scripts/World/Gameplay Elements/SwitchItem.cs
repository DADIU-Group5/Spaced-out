using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwitchItem : MonoBehaviour {

    //Should contain logic on on/off,
    // as well as which object it manipulates.
    //[HideInInspector]
    //public List<GameObject> assignedHazards;

    [Header("How long is the switch untouchable after collision?")]
    public float triggerDelay = 1f;
    
    [Header("Can the switch only be triggered once?")]
    public bool oneTimeTrigger = false;
    
    [Header("Choose on/off colours:")]
    public Color offColour = Color.red;
    public Color onColour = Color.green;

    private bool countingDown = false;
    private bool hasBeenTriggered = false;
    private Renderer switchRenderer;
    private bool on = false;

    Room inRoom;

    void Start()
    {
        switchRenderer = this.GetComponent<Renderer>();
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
        switchRenderer.material.color = on ? onColour : offColour;
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
            }
        }
    }
}
