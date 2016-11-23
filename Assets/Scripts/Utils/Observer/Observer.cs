using UnityEngine;

// Wants to know when another object does something interesting
// Will have to be implemented by manager classes, eg AnimationManager, InputController, SoundEngine etc.
public interface Observer
{
    void OnNotify(GameObject entity, ObserverEvent evt);
    void OnDestroy();
}
