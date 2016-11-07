﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool onFire = false;
    [HideInInspector]
    public bool dead = false;
    
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
        if (rbPlayer.velocity.magnitude > maxMagnitude)
        {
            readyToLaunchText.text = "Velocity: " + rbPlayer.velocity.magnitude + "\nNot Ready To Launch";
        }
        else
        {
            readyToLaunchText.text = "Velocity: " + rbPlayer.velocity.magnitude + "\nReady To Launch";
            UpdateLaunchUI();
        }
    }

    public void Launch(Vector3 direction, float force)
    {
        if (rbPlayer.velocity.magnitude < maxMagnitude)
        {
            Rigidbody body = GetComponent<Rigidbody>();
            body.AddForce(force * direction.normalized);
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
    }

    public void UpdateLaunchUI()
    {
        chargeText.text = "" + launchForce;
        chargeArrow.position = new Vector3(chargeArrow.position.x, chargeArrowYMin + chargeArrowYHeight * launchForce / maxLaunchForce);
    }
}