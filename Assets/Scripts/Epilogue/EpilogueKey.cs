using UnityEngine;
using System.Collections;

public class EpilogueKey : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //TODO: sound. "I will open the door now." (or whatever).

            var evt = new ObserverEvent(EventName.EPILOGUE_EVENTONE);
            Subject.instance.Notify(other.gameObject, evt);

            evt = new ObserverEvent(EventName.ToggleUI);
            Subject.instance.Notify(gameObject, evt);

            evt = new ObserverEvent(EventName.StartCutscene);
            Subject.instance.Notify(gameObject, evt);

            Destroy(gameObject);
        }
    }
}
