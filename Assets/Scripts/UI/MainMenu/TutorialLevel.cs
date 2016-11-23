using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialLevel : MonoBehaviour, Observer
{
    public void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                TutorialComplete();
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void TutorialComplete()
    {
        ProgressManager.instance.completeTutorial();
    }
}
