using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(OxygenController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerControl
{
    [Tooltip("The minimum required force in order to launch. 1 = max launch force.")]
    [Range(0, 1)]
    public float minLaunchPower = 0f;
    public float maxLaunchForce = 60000f;
    [Tooltip("How fast does the player rotate towards aim point")]
    [Range(25f, 200f)]
    public float aimRotateSpeed = 100f;

    // the power of the shot. power is between 0 and 1, where 1 = max launch force
    private float power;
    // the desired rotation
    private Quaternion aim;
    private bool readyForLaunch;
    private Rigidbody body;
    private OxygenController oxygen;



    
    //private bool firedZoomOut = false;
    //private bool canSlowDown = false;

    //public float maxMagnitude = 30f;
    //public Transform pitchTransform;

    // For ensuring that the player at some point starts slowing
    //[Tooltip("Threshold for when velocity is reduced faster.")]
    //public float slowDownThreshold = 2;

    //[Tooltip("How many percent the speed is reduced each update cycle.")]
    //[Range(0,100)]
    //public float slowDownFactor = 2f;

    //[Tooltip("Speed is reduced to 0 when it goes below this value.")]
    //public float slowDownCutOff = 0.2f;
    
    void Awake ()
    {
        readyForLaunch = true;
        aim = transform.rotation;
        body = GetComponent<Rigidbody>();
        oxygen = GetComponent<OxygenController>();
    }

    void Update()
    {
        // rotate player towards aim
        if (transform.rotation != aim)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, aim, aimRotateSpeed * Time.deltaTime);
        }

        //UpdateVelocityUI(rbPlayer.velocity.magnitude.ToString() + "/" + fuelText);


        //if (rbPlayer.velocity.magnitude < slowDownThreshold && firedZoomOut)
        //{
        //    var evt = new ObserverEvent(EventName.CameraZoomIn);
        //    Subject.instance.Notify(gameObject, evt);
        //    firedZoomOut = false;
        //}

        //if (rbPlayer.velocity.magnitude > slowDownThreshold && !firedZoomOut)
        //{
        //    var evt = new ObserverEvent(EventName.CameraZoomOut);
        //    Subject.instance.Notify(gameObject, evt);
        //    firedZoomOut = true;
        //}



        //if (rbPlayer.velocity.magnitude > slowDownThreshold && !canSlowDown)
        //{
        //    canSlowDown = true;
        //}

        //// Slows the player down faster when his velocity is below slowDownThreshold
        //if (rbPlayer.velocity.magnitude < slowDownThreshold && canSlowDown)
        //{
        //    rbPlayer.velocity = rbPlayer.velocity * (100f - slowDownFactor) / 100f;
        //}

        //// If velocity is below the set threshold, set to zero.
        //if (rbPlayer.velocity.magnitude < slowDownCutOff)
        //{
        //    rbPlayer.velocity = Vector3.zero;
        //    canSlowDown = false;

        //    if (!oxygen.HasOxygen())
        //    {
        //        var evt = new ObserverEvent(EventName.FuelEmpty);
        //        Subject.instance.Notify(gameObject, evt);
        //    }
        //}
    }

    // set the controller to ready for launch
    public void ReadyForLaunch()
    {
        readyForLaunch = true;
    }

    // aim the player at a certain point in world space
    public void Aim(Vector3 point)
    {
        if (!readyForLaunch)
            return;

        Vector3 direction = (point - transform.position).normalized;
        aim = Quaternion.LookRotation(direction);
    }

    // set power for next launch
    public void SetPower(float power)
    {
        if (!readyForLaunch)
            return;

        this.power = Mathf.Clamp01(power);
        ThrowLaunchPowerChangedEvent();
    }

    // launch the player
    public void Launch()
    {
        if (!readyForLaunch || power < minLaunchPower)
            return;
        
        // perform the launch
        body.AddForce(power * maxLaunchForce * transform.forward);
        oxygen.UseOxygen();
        ThrowLaunchEvent();
        SetPower(0);
    }

    private void ThrowLaunchPowerChangedEvent()
    {
        var evt = new ObserverEvent(EventName.LaunchPowerChanged);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, new Vector2(power * maxLaunchForce, maxLaunchForce));
        Subject.instance.Notify(gameObject, evt);
    }

    private void ThrowLaunchEvent()
    {
        var evt = new ObserverEvent(EventName.PlayerLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, power * maxLaunchForce);
        evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, transform.forward);
        Subject.instance.Notify(gameObject, evt);
    }
}