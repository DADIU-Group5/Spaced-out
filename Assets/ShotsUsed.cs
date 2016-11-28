using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ShotsUsed : MonoBehaviour, Observer
{
    int shotsUsed = 0;
    public Text shotText;

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if(evt.eventName == EventName.PlayerLaunch)
        {
            shotsUsed++;
            UpdateText();
        }
        
    }

    void UpdateText()
    {
        shotText.text = "Shots used: " + shotsUsed;
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
