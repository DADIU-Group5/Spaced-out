using UnityEngine;
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
    public GameplayElement itemState;

    public void EnabledOrDisableTrap()
    {
        isOn = !isOn;
        TagChanger();

        if (isOn)
        {
            if (gameObject.GetComponent<Renderer>() != null)
                GetComponent<Renderer>().material.color = orgColour;
        }
        else
        {
            if (gameObject.GetComponent<Renderer>() != null)
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
        if (gameObject.GetComponent<Renderer>() != null)
        orgColour = GetComponent<Renderer>().material.color;
        itemState = this.GetComponent<GameplayElement>();
        behaviour = itemState.behaviour;

    }
}
