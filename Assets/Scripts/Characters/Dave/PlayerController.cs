using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour, Observer
{
    [HideInInspector]
    public bool onFire = false;
    [HideInInspector]
    private bool dead = false;
    
    private float launchForce = 0f;
    private bool firedZoomOut = false;
    private bool canSlowDown = false;

    public float minLaunchForce = 0f, maxLaunchForce = 3000f,  maxMagnitude = 30f;
    public Transform pitchTransform;
    public Rigidbody rbPlayer;
    public FuelController fuel;

    // For ensuring that the player at some point starts slowing
    [Tooltip("Threshold for when velocity is reduced faster.")]
    public float slowDownThreshold = 2;

    [Tooltip("How many percent the speed is reduced each update cycle.")]
    [Range(0,100)]
    public float slowDownFactor = 2f;

    [Tooltip("Speed is reduced to 0 when it goes below this value.")]
    public float slowDownCutOff = 0.2f;
    
    void Awake ()
    {
        Subject.instance.AddObserver(this);
    }

    public float GetMinLaunchForce()
    {
        return minLaunchForce;
    }

    public float GetMaxLaunchForce()
    {
        return maxLaunchForce;
    }

    private void Update()
    {
        if (!fuel.HasFuel())
        {
            UpdateVelocityUI("Velocity: " + rbPlayer.velocity.magnitude + "\nNo More Fuel");
        }
        else if (rbPlayer.velocity.magnitude > maxMagnitude)
        {
            UpdateVelocityUI("Velocity: " + rbPlayer.velocity.magnitude + "\nNot Ready To Launch");
        }
        else
        {
            UpdateVelocityUI("Velocity: " + rbPlayer.velocity.magnitude + "\nReady To Launch");
        }
        


        if (rbPlayer.velocity.magnitude < slowDownThreshold && firedZoomOut)
        {
            var evt = new ObserverEvent(EventName.CameraZoomIn);
            Subject.instance.Notify(gameObject, evt);
            firedZoomOut = false;
        }

        if (rbPlayer.velocity.magnitude > slowDownThreshold && !firedZoomOut)
        {
            var evt = new ObserverEvent(EventName.CameraZoomOut);
            Subject.instance.Notify(gameObject, evt);
            firedZoomOut = true;
        }



        if (rbPlayer.velocity.magnitude > slowDownThreshold && !canSlowDown)
        {
            canSlowDown = true;
        }

        // Slows the player down faster when his velocity is below slowDownThreshold
        if (rbPlayer.velocity.magnitude < slowDownThreshold && canSlowDown)
        {
            rbPlayer.velocity = rbPlayer.velocity * (100f - slowDownFactor) / 100f;
        }

        // If velocity is below the set threshold, set to zero.
        if (rbPlayer.velocity.magnitude < slowDownCutOff && canSlowDown)
        {
            rbPlayer.velocity = Vector3.zero;
            canSlowDown = false;
        }
    }

    public void Launch(float force, Vector3 direction)
    {
        if (rbPlayer.velocity.magnitude < maxMagnitude && force > 0)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.AddForce(force * maxLaunchForce * direction.normalized);
            fuel.UseFuel();
        }
        launchForce = 0;
        UpdateLaunchUI();
    }

    public void Launch(float force)
    {
        Launch(force, pitchTransform.forward);
    }

    public void SetLaunchForce(float force)
    {
        if (fuel.HasFuel() && rbPlayer.velocity.magnitude < maxMagnitude)
        {
            launchForce = force * maxLaunchForce;
            UpdateLaunchUI();
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public void UpdateLaunchUI()
    {
        var evt = new ObserverEvent(EventName.UpdateLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, new Vector2(launchForce, maxLaunchForce));
        Subject.instance.Notify(gameObject, evt);
    }

    private void UpdateVelocityUI(string text)
    {
        var evt = new ObserverEvent(EventName.UpdateVelocity);
        evt.payload.Add(PayloadConstants.VELOCITY, text);
        Subject.instance.Notify(gameObject, evt);
    }

    /*internal void Kill()
    {
        dead = true;
        StartCoroutine(gameOverMenu.GameOver());
    }*/

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerLaunch:
                var payload = evt.payload;
                float launchForce = (float)payload[PayloadConstants.LAUNCH_FORCE];
                Vector3 launchDirection = (Vector3)payload[PayloadConstants.LAUNCH_DIRECTION];
                Launch(launchForce, launchDirection);
                break;
            /*case EventName.PlayerDead:
                Debug.Log("calling on notify");
                if (!dead)
                    Kill(); //this keeps calling?*/
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}