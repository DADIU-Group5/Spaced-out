using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OxygenHUD : MonoBehaviour, Observer
{
    public List<Image> bars;

    public void Start()
    {
        Subject.instance.AddObserver(this);
        /*
        oxygenRenderers = new List<RawImage>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            oxygenRenderers.Add(transform.GetChild(i).GetComponent<RawImage>());
        }
        */
    }

    private void UpdateOxygenMeter(int oxygen)
    {
        int currentBar = 0;
        for (int i = 0; i < bars.Count; i++, currentBar += 2)
        {
            if (currentBar + 2 <= oxygen)
            {
                bars[i].enabled = true;
                bars[i].color = Color.blue;
            }
            else if (currentBar + 1 == oxygen)
            {
                bars[i].enabled = true;
                bars[i].color = Color.red;
            }
            else
            {
                bars[i].enabled = false;
                bars[i].color = Color.clear;
            }
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.UpdateOxygen:
                var payload = evt.payload;
                int oxygen = (int)payload[PayloadConstants.OXYGEN];
                UpdateOxygenMeter(oxygen);
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}