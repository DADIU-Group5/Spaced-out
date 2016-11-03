﻿using UnityEngine;
using System.Collections;

public class HazardState : MonoBehaviour {

    [Header("A switch can set a hazard On/Off")]
    
    [Tooltip("Only change this through script :)")]
    public bool isOn = true;

    public GameObject hazardSwitch;

    private Behaviour behaviour;

    private Color orgColour;
    private Color flashColour = Color.cyan;

    [HideInInspector]
    public Item itemState;

    public void EnabledOrDisableTrap()
    {
        isOn = !isOn;
        TagChanger();

        if (isOn)
        {
            GetComponent<Renderer>().material.color = orgColour;
        }
        else
        {
            GetComponent<Renderer>().material.color = flashColour;
        }

        Debug.Log("An object was turned On: "+ isOn);
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
        orgColour = GetComponent<Renderer>().material.color;
        itemState = this.GetComponent<Item>();
        behaviour = itemState.behaviour;

    }
}
