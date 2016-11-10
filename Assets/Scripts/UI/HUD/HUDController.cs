using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour, Observer {

    public Text fuelText;
    public Text chargeText;
    public Text launchText;
    public Text statusText;
    public Transform chargeArrow;

    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    private bool gameOver = false;
    private bool OnFire = false;

    void Awake ()
    {
        Subject.instance.AddObserver(this);
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
                string velocity = "";
                velocity = (string)velocityPayload[PayloadConstants.VELOCITY];

                launchText.text = velocity;

                break;

            case EventName.UpdateStatus:
                //only allow status change if:
                //game is not over, OR we're on fire (allowed)
                if (!gameOver || OnFire)
                {
                    var statusPayload = evt.payload;
                    string status = (string)statusPayload[PayloadConstants.STATUS];

                    statusText.text = status;
                }
                break;
            case EventName.PlayerDead:
                gameOver = true;
                break;
            case EventName.PlayerWon:
                gameOver = true;
                break;
            case EventName.OnFire:
                if (!gameOver)
                    OnFire = true;
                break;
            case EventName.Extinguish:
                OnFire = false;
                break;

            default:
                break;
        }
    }
}
