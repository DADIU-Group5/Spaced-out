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

    public float minLaunchForce = 0f, maxLaunchForce = 3000f, launchSideScale = 10f;
    public float maxMagnitude = 30f;
    public Transform pitchTransform;
    public Rigidbody rbPlayer;
    public FuelController fuel;

    // Use this for initialization
    void Start()
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
    }

    public void Launch(Vector3 direction, float force)
    {
        if (rbPlayer.velocity.magnitude < maxMagnitude)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.AddForce(force * direction.normalized);

            if (force > 0)
            {
                fuel.UseFuel();
            }
        }
    }

    public void Launch(float force)
    {
        launchForce = force * maxLaunchForce;
        Launch(pitchTransform.forward, launchForce);
        launchForce = 0;

        UpdateLaunchUI();
    }

    public void SetLaunchForce(float force)
    {
        launchForce = force * maxLaunchForce;
        UpdateLaunchUI();
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
                float launchForce = (float)payload[PayloadConstants.LAUNCH_SPEED];
               
                Launch(launchForce);

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
}