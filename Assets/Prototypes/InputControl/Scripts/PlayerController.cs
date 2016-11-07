﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool onFire = false;
    [HideInInspector]
    public bool Dead = false;

    private bool charging = false, increasing = false;
    private float launchForce = 0f;

    public float minLaunchForce = 0f, maxLaunchForce = 3000f, launchSideScale = 10f;
    public Transform pitchTransform;
    public Text chargeText;
    public Transform chargeArrow;
    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    private bool holdControl = true;

    public void PlayerDied()
    {
        Dead = true;
        StartCoroutine(GameObject.Find("GameOverCanvas").GetComponent<GameOverMenu>().GameOver());
    }

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
        holdControl = input;
    }

    private void Update()
    {
        if (Dead)
        {
            PlayerDied();
            Dead = !Dead;
        }

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
        //body.velocity += force * direction.normalized / 100;
    }

    public void Launch(float force)
    {
        launchForce = force;
        LaunchCharge();
        launchForce = 0;
    }

    public void SetLaunchForce(float force)
    {
        launchForce = force;
    }
}