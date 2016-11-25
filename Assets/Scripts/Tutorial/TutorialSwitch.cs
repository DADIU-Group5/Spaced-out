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
            }
        }
    }
}
