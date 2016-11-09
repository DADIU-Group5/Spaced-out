using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour, Observer {

    private Text fuelText;
    private Text chargeText;
    private Text launchText;
    private Transform chargeArrow;

    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    void Start () {
        Subject.instance.AddObserver(this);

        // These are all in the this prefab, so there will not be NULL references unless the prefab is broken..
        fuelText = GameObject.Find("Fuel Text").GetComponent<Text>();
        chargeText = GameObject.Find("Charge Text").GetComponent<Text>();
        launchText = GameObject.Find("Launch Text").GetComponent<Text>();
        chargeArrow = GameObject.Find("Charge Arrow").GetComponent<Transform>();
    }
	
	void Update () {

    }

    public void ToggleCameraControls()
    {
        var evt = new ObserverEvent(EventName.ToggleCameraControls);
        Subject.instance.Notify(gameObject, evt);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.UpdateFuel:
                var fuelPayload = evt.payload;
                int fuel = (int)fuelPayload[PayloadConstants.FUEL];

                fuelText.text = "Current fuel: " + fuel;

                break;

            case EventName.UpdateLaunch:
                var launchPayload = evt.payload;
                Vector2 launch = (Vector2)launchPayload[PayloadConstants.LAUNCH_FORCE];

                chargeText.text = launch.x.ToString();
                chargeArrow.position = new Vector3(chargeArrow.position.x, chargeArrowYMin + chargeArrowYHeight * launch.x / launch.y);

                break;

            case EventName.UpdateVelocity:
                var velocityPayload = evt.payload;
                string velocity = (string)velocityPayload[PayloadConstants.VELOCITY];
                
                launchText.text = velocity;

                break;
            default:
                break;
        }
    }
}
