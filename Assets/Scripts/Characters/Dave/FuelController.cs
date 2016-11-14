using UnityEngine;
using System.Collections;

public class FuelController : MonoBehaviour
{
    public int maxFuel = 10;
    public float velocityToDie = 10f;

    private int currentFuel;
    private Rigidbody rbPlayer;
    private PlayerController player;

    public void Start()
    {
        player = gameObject.GetComponent<PlayerController>();
        rbPlayer = gameObject.GetComponent<Rigidbody>();

        ReplenishFuel();
    }

    public void Update()
    {
        // Check if player is traveling slowly and also has no more fuel, and if so, fire PlayerDead event.
        if (rbPlayer.velocity.magnitude < velocityToDie && currentFuel <= 0 && !player.IsDead())
        {
            var evt = new ObserverEvent(EventName.FuelEmpty);
            Subject.instance.Notify(gameObject, evt);
        }
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
