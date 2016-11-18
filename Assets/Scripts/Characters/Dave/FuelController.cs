using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour
{
    public int maxFuel = 10;
    public float velocityToDie = 10f;

    private int currentFuel = 5;
    private Rigidbody rbPlayer;
    private PlayerController player;

    public void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
        rbPlayer = gameObject.GetComponent<Rigidbody>();
    }

    public void UseFuel()
    {
        currentFuel--;
        UpdateFuelUI();
    }

    public bool HasFuel()
    {
        return currentFuel > 0;
    }

    public int GetCurrentFuel()
    {
        return currentFuel;
    }

    // Sets the fuel to the given value within allowed limits.
    public void SetFuel(int fuel)
    {
        this.currentFuel = Mathf.Clamp(fuel, 0, maxFuel);
        UpdateFuelUI();
    }

    public void ReplenishFuel()
    {
        currentFuel = maxFuel;
        UpdateFuelUI();
    }

    private void UpdateFuelUI()
    {
        var evt = new ObserverEvent(EventName.UpdateFuel);
        evt.payload.Add(PayloadConstants.FUEL, currentFuel);
        Subject.instance.Notify(gameObject, evt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fuel Pickup"))
        {
            ReplenishFuel();
            Destroy(other.gameObject);

            var evt = new ObserverEvent(EventName.PlayerFuelPickup);
            Subject.instance.Notify(gameObject, evt);
        }
    }
}
