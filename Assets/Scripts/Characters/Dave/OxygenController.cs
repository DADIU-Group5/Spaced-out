using UnityEngine;
using System.Collections;

public class OxygenController : MonoBehaviour
{
    public int maxOxygen = 10;
    private int oxygen;

    public void Start()
    {
        oxygen = maxOxygen;
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

    // throw oxygen event
    private void ThrowOxygenChangedEvent()
    {
        var evt = new ObserverEvent(EventName.UpdateOxygen);
        evt.payload.Add(PayloadConstants.Oxygen, oxygen);
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
