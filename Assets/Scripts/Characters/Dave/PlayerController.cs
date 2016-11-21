using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public interface IPlayerControl
{
    void Aim(Vector3 point);
    // 0 = min power, 1 = max power
    void SetPower(float power);
    void Launch();
}

/// <summary>
/// This class is responsible for controlling the player launches.
/// Input controller should only comunicate through the above interface
/// </summary>
[RequireComponent(typeof(OxygenController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerControl
{
    [Tooltip("The minimum required power in order to launch. 1 = max launch velocity.")]
    [Range(0, 1)]
    public float minLaunchPower = 0.05f;
    [Tooltip("The maximum velocity change from launch.")]   
    [Range(5, 30)]
    public float maxLaunchVelocity = 20f;
    [Tooltip("How fast does the player rotate towards aim point")]
    [Range(25f, 200f)]
    public float aimRotateSpeed = 100f;

    // the power of the shot. power is between 0 and 1, where 1 = max launch velocity
    private float power;
    // the desired rotation
    private Quaternion aim;
    private bool readyForLaunch;
    private Rigidbody body;
    private OxygenController oxygen;
    private Animator animator;
    
    void Awake ()
    {
        readyForLaunch = true;
        aim = transform.rotation;
        body = GetComponent<Rigidbody>();
        oxygen = GetComponent<OxygenController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // rotate player towards aim
        if (transform.rotation != aim)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, aim, aimRotateSpeed * Time.deltaTime);
        }
    }

    // set the controller to ready for launch
    public void ReadyForLaunch()
    {
        if (oxygen.HasOxygen())
        {
            readyForLaunch = true;
            animator.SetTrigger("Ready To Launch");
        }
        else
        {
            // throw oxygen death event
            animator.SetTrigger("Death Oxygen");
            var evt = new ObserverEvent(EventName.OxygenEmpty);
            Subject.instance.Notify(gameObject, evt);
        }

    }

    public void BurningToDeath()
    {
        animator.SetTrigger("Death Fire");
        Debug.Log("setting trigger for fire death");
    }

    public void ElectrocutedToDeath()
    {
        animator.SetTrigger("Death Electricity");
    }

   /* public void ChokingToDeath()
    {
        animator.SetTrigger("Death Fire");
    }*/

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
        animator.SetBool("Launch Mode", true);
        animator.SetFloat("Power", power);
        ThrowLaunchPowerChangedEvent();
        ThrowChargingPowerEvent(true);
    }

    // launch the player
    public void Launch()
    {
        if (!readyForLaunch || power < minLaunchPower)
            return;

        // perform the launch
        Vector3 dir = aim * Vector3.forward;
        body.AddForce(power * maxLaunchVelocity * dir, ForceMode.VelocityChange);
        oxygen.UseOxygen();
        animator.SetBool("Launch Mode", false);

        ThrowLaunchEvent();

        // reset power
        power = 0;
        ThrowLaunchPowerChangedEvent();
        ThrowChargingPowerEvent(false);
        readyForLaunch = false;
    }

    private void ThrowLaunchPowerChangedEvent()
    {
        var evt = new ObserverEvent(EventName.LaunchPowerChanged);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, new Vector2(power * maxLaunchVelocity, maxLaunchVelocity));
        Subject.instance.Notify(gameObject, evt);
        // TODO sound
    }

    private void ThrowLaunchEvent()
    {
        var evt = new ObserverEvent(EventName.PlayerLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, power * maxLaunchVelocity);
        evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, transform.forward);
        Subject.instance.Notify(gameObject, evt);
        // TODO sound
    }

    private void ThrowChargingPowerEvent(bool start)
    {
        var evt = new ObserverEvent(EventName.PlayerCharge);
        evt.payload.Add(PayloadConstants.START_STOP, start);
        Subject.instance.Notify(gameObject, evt);
    }
}