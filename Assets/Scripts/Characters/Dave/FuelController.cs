using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FuelController : MonoBehaviour {

    private int currentFuel;
    
    public int maxFuel = 10;
    public float velocityToDie = 10f;

    public Text fuelText;
    public Rigidbody rbPlayer;
    public PlayerBehaviour player;
    private bool dead = false;


    public void Awake()
    {
        ReplenishFuel();
    }

    public void Update()
    {
        if (rbPlayer.velocity.magnitude < velocityToDie && currentFuel <= 0 && !dead)
        {
            var evt = new ObserverEvent(EventName.FuelEmpty); //fuelEmpty
            Subject.instance.Notify(gameObject, evt);
            dead = true;
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

    public void ReplenishFuel()
    {
        currentFuel = maxFuel;
        UpdateFuelUI();
    }

    public void UpdateFuelUI()
    {
        fuelText.text = "Current fuel: " + currentFuel;
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
