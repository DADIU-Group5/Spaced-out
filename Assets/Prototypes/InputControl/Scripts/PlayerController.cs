using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool charging = false, increasing = false;
    private float launchForce = 0f;
    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    public float minLaunchForce = 0f, maxLaunchForce = 3000f, launchSideScale = 10f;
    public float maxMagnitude = 30f;
    public Transform pitchTransform;
    public Text chargeText;
    public Text readyToLaunchText;
    public Transform chargeArrow;
    public Rigidbody rbPlayer;

    private bool holdControl = true;

    public float GetMinLaunchForce()
    {
        return minLaunchForce;
    }

    public float GetMaxLaunchForce()
    {
        return maxLaunchForce;
    }

    public void SetHoldControl(bool input)
    {
        if (rbPlayer.velocity.magnitude < maxMagnitude)
        {
            holdControl = input;
        }
    }

    private void Update()
    {
        if (rbPlayer.velocity.magnitude > maxMagnitude)
        {
            readyToLaunchText.text = "Velocity: " + rbPlayer.velocity.magnitude + "\nNot Ready To Launch";
        }
        else
        {
            readyToLaunchText.text = "Velocity: " + rbPlayer.velocity.magnitude + "\nReady To Launch";
            
            if (holdControl)
            {
                if (charging)
                {
                    Charge();
                }
                else
                {
                    launchForce = 0;
                }
            }
            chargeText.text = "" + launchForce;
            chargeArrow.position = new Vector3(chargeArrow.position.x, chargeArrowYMin + chargeArrowYHeight * launchForce / maxLaunchForce);
        }
    }

    // Deprecated! No sure if I should delete... /Malte
    private void Charge()
    {
        float delta = (Time.deltaTime * (maxLaunchForce - minLaunchForce));
        if (increasing)
        {
            launchForce += delta;
            if (launchForce > maxLaunchForce)
            {
                launchForce = 2 * maxLaunchForce - launchForce;
                increasing = false;
            }
        }
        else
        {
            launchForce -= delta;
            if (launchForce < minLaunchForce)
            {
                launchForce = 2 * minLaunchForce - launchForce;
                increasing = true;
            }
        }
    }

    // Deprecated! No sure if I should delete... /Malte
    public void SetCharging(bool value)
    {
        charging = value;
        if (charging)
        {
            launchForce = minLaunchForce;
            increasing = true;
        }
        else
        {
            launchForce = 0f;
        }
    }

    public void LaunchCharge()
    {
        LaunchCharge(pitchTransform.forward);
    }

    public void LaunchCharge(Vector3 direction)
    {
        Launch(direction, launchForce);
    }

    public void LaunchScale(Vector3 distance)
    {
        Launch(distance, distance.magnitude * launchSideScale);
    }

    public void Launch(Vector3 direction, float force)
    {
        if (rbPlayer.velocity.magnitude < maxMagnitude) {
            Rigidbody body = GetComponent<Rigidbody>();
            body.AddForce(force * direction.normalized);
        }
    }

    public void Launch(float force)
    {
        launchForce = force * maxLaunchForce;
        LaunchCharge();
        launchForce = 0;
    }

    public void SetLaunchForce(float force)
    {
        launchForce = force * maxLaunchForce;
    }
}