using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OxygenController : MonoBehaviour
{
    public int maxOxygen = 10;
    private int oxygen;
    [HideInInspector]
    public bool godMode = false;

    public bool fuelGodMode = false;

    public void Start()
    {
        oxygen = maxOxygen;
        ThrowOxygenChangedEvent();
    }

    public void UseOxygen()
    {
        if (!godMode && !fuelGodMode)
        {
            oxygen--;
            ThrowOxygenChangedEvent();
        }
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

    // throw oxygen event
    private void ThrowOxygenChangedEvent()
    {
        var evt = new ObserverEvent(EventName.UpdateOxygen);
        evt.payload.Add(PayloadConstants.OXYGEN, oxygen);
        Subject.instance.Notify(gameObject, evt);
    }

    // when hitting oxygen pickup
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fuel Pickup"))
        {
            var evt = new ObserverEvent(EventName.PlayerFuelPickup);
            Subject.instance.Notify(gameObject, evt);
            ReplenishOxygen();
            Destroy(other.gameObject);
        }
    }
}