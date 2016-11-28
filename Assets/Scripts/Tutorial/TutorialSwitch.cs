using UnityEngine;
using System.Collections;

public class TutorialSwitch : MonoBehaviour {

    public HazardState[] hazards;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
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
