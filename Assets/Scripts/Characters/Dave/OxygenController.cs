using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OxygenController : MonoBehaviour
{
    public int maxOxygen = 10;
    private int oxygen;

    public GameObject oxygenObject;
    private List<Renderer> oxygenRenderers;

    public void Awake()
    {
        oxygen = maxOxygen;
        


        oxygenRenderers = new List<Renderer>(oxygenObject.transform.childCount);
        for (int i = 0; i < oxygenObject.transform.childCount; i++)
        {
            oxygenRenderers.Add(oxygenObject.transform.GetChild(i).GetComponent<Renderer>());
        }
        UpdateOxygenMeter();
    }

    public void UseOxygen()
    {
        oxygen--;
        ThrowOxygenChangedEvent();
    }

    public bool HasOxygen()
    {
        return oxygen > 0;
    }
    
    public int GetOxygen()
    {
        return oxygen;
    }

    /// <summary>
    /// Sets the fuel to the given value within allowed limits.
    /// </summary>
    /// <param name="oxygen">Amount of oxygen</param>
    public void SetOxygen(int oxygen)
    {
        this.oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);
        ThrowOxygenChangedEvent();
    }

    /// <summary>
    /// Resets the oxygen back to max
    /// </summary>
    public void ReplenishOxygen()
    {
        oxygen = maxOxygen;
        ThrowOxygenChangedEvent();
    }

    private void UpdateOxygenMeter()
    {
        int currentBar = 0;
        for (int i = 0; i < oxygenRenderers.Count; i++, currentBar += 2)
        {
            if (currentBar + 2 <= oxygen)
            {
                oxygenRenderers[i].material.color = Color.blue;
                oxygenRenderers[i].gameObject.SetActive(true);
            }
            else if (currentBar + 1 == oxygen)
            {
                oxygenRenderers[i].material.color = Color.red;
                oxygenRenderers[i].gameObject.SetActive(true);
            }
            else {
                oxygenRenderers[i].gameObject.SetActive(false);
            }
        }
        
    }

    // throw oxygen event
    private void ThrowOxygenChangedEvent()
    {
        UpdateOxygenMeter();

        var evt = new ObserverEvent(EventName.UpdateOxygen);
        evt.payload.Add(PayloadConstants.OXYGEN, oxygen);
        Subject.instance.Notify(gameObject, evt);
    }

    // when hitting oxygen pickup
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Oxygen"))
        {
            ReplenishOxygen();
            Destroy(other.gameObject);
        }
    }
}
