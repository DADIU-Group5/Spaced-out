using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaunchMeterController : MonoBehaviour, Observer
{
    public List<Renderer> bars;
    public Material newMat;
    private Material OrgMat;
    float delimiter;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
        OrgMat = bars[0].material;
    }

    private void Start()
    {
        delimiter = 1f / 8f; // Change this if we have more or fewer than 8 bars
        UpdateBars(0f);
    }

    private void UpdateBars(float t)
    {
        float current = 0f;
        for (int i = 0; i < bars.Count; i++, current+= delimiter)
        {
            if (current < t)
            {
                bars[i].material = newMat;
                if (i <= 1)
                {
                    bars[i].material.color = new Color(34f / 225f, 177f / 225f, 76f / 225f); // Green
                }
                else if (i <= 3)
                {
                    bars[i].material.color = new Color(1f, 242f / 225f, 0f); // Yellow
                }
                else if (i <= 5)
                {
                    bars[i].material.color = new Color(1f, 127f / 225f, 39f / 225f); // Orange
                }
                else if (i <= 7)
                {
                    bars[i].material.color = new Color(237f / 225f, 28f / 225f, 36f / 225f); // Red
                }
            }
            else
            {
                bars[i].material = OrgMat;
                bars[i].material.color = new Color(63f / 225f, 72f / 225f, 204f / 225f);
            }
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.LaunchPowerChanged:
                var launchPayload = evt.payload;
                Vector2 launch = (Vector2)launchPayload[PayloadConstants.LAUNCH_FORCE];
                float t = launch.x / launch.y;
                UpdateBars(t);
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}



            

