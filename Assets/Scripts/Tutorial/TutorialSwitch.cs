using UnityEngine;
using System.Collections;

public class TutorialSwitch : MonoBehaviour {

    public HazardState[] hazards;
    public Color offColour = Color.red;
    public Color onColour = Color.green;
    private Renderer switchRenderer;
    private bool on = false;

    void Start()
    {
        switchRenderer = this.GetComponent<Renderer>();
        SwitchColor();
    }

    //switch the color
    void SwitchColor()
    {
        switchRenderer.material.color = on ? onColour : offColour;
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
