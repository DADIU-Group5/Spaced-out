using UnityEngine;

public class TutorialSwitch : MonoBehaviour
{
    public HazardState[] hazards;
    public Color offColour = Color.red;
    public Color onColour = Color.green;
    public Renderer switchRenderer;
    private bool on = false;
    public GameObject actualButtonPart;

    void Start()
    {
        if (actualButtonPart != null)
            switchRenderer = actualButtonPart.GetComponent<Renderer>();
        else
            Debug.Log("Couldn't find button in switch prefab. Did you remember to drag & drop it?");
        SwitchColor();
    }

    //switch the color
    void SwitchColor()
    {
        on = !on;
        if (actualButtonPart != null)
            switchRenderer.material.color = on ? onColour : offColour;
        else
            Debug.Log("Couldn't find button in switch prefab");
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            SwitchColor();

            foreach (HazardState state in hazards)
            {
                state.EnabledOrDisableTrap();

                var evt = new ObserverEvent(EventName.SwitchPressed);
                evt.payload.Add(PayloadConstants.SWITCH_ON, on);
                Subject.instance.Notify(gameObject, evt);
            }
        }
    }
}
