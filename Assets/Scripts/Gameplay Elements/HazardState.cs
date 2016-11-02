using UnityEngine;
using System.Collections;

public class HazardState : MonoBehaviour {

    [Header("A switch can set a hazard On/Off")]
    
    [Tooltip("Only change this through script :)")]
    public bool isOn = true;

    public GameObject hazardSwitch;

    private Behaviour behaviour;

    [HideInInspector]
    public Item itemState;

    public void EnabledOrDisableTrap()
    {
        isOn = !isOn;
    }

    void TagChanger()
    {
        if (isOn)
            itemState.behaviour = Behaviour.none;
        else
            itemState.behaviour = behaviour;
    }

    // Use this for initialization
    void Start()
    {

        itemState = this.GetComponent<Item>();
        behaviour = itemState.behaviour;

    }
}
