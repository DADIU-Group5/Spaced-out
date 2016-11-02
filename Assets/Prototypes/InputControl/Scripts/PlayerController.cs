using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool charging = false, increasing = false;
    private float launchForce = 0f;

    public float minLaunchForce = 100f, maxLaunchForce = 200f, launchSideScale = 10f, timeToMax = 1f;
    public Transform pitchTransform;
    public Text chargeText;
    public Transform chargeArrow;
    float chargeArrowYMin = -700.0f;
    float chargeArrowYHeight = 400.0f;

    private void Update()
    {
        if (charging)
        {
            Charge();
        }
        else
        {
            launchForce = 0;
        }
        chargeText.text = "" + launchForce;

        chargeArrow.transform.position = new Vector3(chargeArrow.position.x, chargeArrowYMin + chargeArrowYHeight * launchForce / maxLaunchForce);
    }

    private void Charge()
    {
        float delta = (Time.deltaTime * (maxLaunchForce - minLaunchForce)) / timeToMax;
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
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(force * direction.normalized);
    }
}