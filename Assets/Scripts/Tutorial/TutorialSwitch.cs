using UnityEngine;
using System.Collections;

public class TutorialSwitch : MonoBehaviour {

    public HazardState[] hazards;
    public Color offColour = Color.red;
    public Color onColour = Color.green;
    public GameObject actualButtonPart;
    private Renderer switchRenderer;
    private bool on = false;

    void Start()
    {
        if (actualButtonPart != null)
            switchRenderer = actualButtonPart.GetComponent<Renderer>();
        else
            Debug.Log("Couldn't find button in switch prefab");
        SwitchColor();
    }

    //switch the color
    void SwitchColor()
    {
        if (actualButtonPart != null)
            switchRenderer.material.color = on ? onColour : offColour;
        else
            Debug.Log("Couldn't find button in switch prefab");
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            on = !on;
            SwitchColor();
            
            foreach (HazardState state in hazards)
            {
                state.EnabledOrDisableTrap();

                var evt = new ObserverEvent(EventName.SwitchPressed);
                evt.payload.Add(PayloadConstants.SWITCH_ON, false);
                Subject.instance.Notify(gameObject, evt);
            }
        }
    }
}
