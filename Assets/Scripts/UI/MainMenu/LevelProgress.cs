using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour, Observer
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

    public void TutorialComplete()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
    }
}
