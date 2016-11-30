using UnityEngine;

public interface IPlayerControl
{
    void Aim(Vector3 point);
    void ReadyForLaunch();
    void SetPower(float power); // 0 = min power, 1 = max power
    void Launch();
}

/// <summary>
/// This class is responsible for controlling the player launches.
/// Input controller should only comunicate through the above interface
/// </summary>
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
    public GameObject chargeParticle;

    // the power of the shot. power is between 0 and 1, where 1 = max launch velocity
    private float power;
    // the desired rotation
    private Quaternion aim;
    private bool readyForLaunch;
    private Rigidbody body;
    private Animator animator;
    private InputController inputCont;
    private bool playMusic = true;

    void Awake ()
    {
        readyForLaunch = true;
        aim = transform.rotation;
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        
        if (Camera.main.transform.parent) {
            inputCont = Camera.main.transform.parent.parent.GetComponent<InputController>();
        }

        // update the random factor in animator periodically
        InvokeRepeating("RandomizeAnimator", 0, 1.5f);
    }

    void Update()
    {

        // rotate player towards aim
        if (transform.rotation != aim)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, aim, aimRotateSpeed * Time.deltaTime);
        }
        if (chargeParticle != null)
        {
            if (power > 0)
            {
                if (!chargeParticle.activeSelf)
                {
                    chargeParticle.SetActive(true);
                }
            }
            else
            {
                if (chargeParticle.activeSelf)
                {
                    chargeParticle.SetActive(false);
                }
            }
        }

        // update velocity in animator to make the fly animation adjust to it
        if (!readyForLaunch)
        {
            float velocity = Mathf.Lerp(0.6f, 1f, body.velocity.magnitude / maxLaunchVelocity);
            animator.SetFloat("Velocity", velocity);
        }
            
    }

    // randomizes idle and fly animations to make them look less repeatable
    private void RandomizeAnimator()
    {
        animator.SetInteger("Random", UnityEngine.Random.Range(0, 7));
    }

    /// <summary>
    ///  set the player to ready for launch
    /// </summary>
    public void ReadyForLaunch()
    {
        readyForLaunch = true;
        animator.SetTrigger("Ready To Launch");
        Aim(inputCont.GetAimPoint());
        var evt = new ObserverEvent(EventName.PlayerReadyForLaunch);
        Subject.instance.Notify(gameObject, evt);
    }

    // aim the player at a certain point in world space
    public void Aim(Vector3 point)
    {
        if (!readyForLaunch)
            return;
        Vector3 direction = (point - transform.position).normalized;
        aim = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// set power for next launch
    /// </summary>
    /// <param name="power">0 = min velocity, 1 = max velocity</param>
    public void SetPower(float power)
    {
        if (!readyForLaunch)
            return;

        power = Mathf.Ceil(power * 8f) / 8f;

        this.power = Mathf.Clamp01(power);
        animator.SetFloat("Power", power);
        ThrowLaunchPowerChangedEvent();
        ThrowChargingPowerEvent(true, this.power);
    }

    /// <summary>
    /// launch the player
    /// </summary>
    public void Launch()
    {
        if (!readyForLaunch || power < minLaunchPower)
            return;

        // perform the launch
        Vector3 dir = aim * Vector3.forward; //inputCont.GetLaunchDirection();
        body.AddForce(power * maxLaunchVelocity * dir, ForceMode.VelocityChange);
        animator.SetTrigger("Launch");
        ThrowLaunchEvent();
        SetPower(0);
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
        evt.payload.Add(PayloadConstants.START_STOP, playMusic);
        playMusic = false;
        Subject.instance.Notify(gameObject, evt);
        // TODO sound
    }

    private void ThrowChargingPowerEvent(bool start, float force = 0)
    {
        var evt = new ObserverEvent(EventName.PlayerCharge);
        evt.payload.Add(PayloadConstants.START_STOP, start);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, force * 100);
        Subject.instance.Notify(gameObject, evt);
    }

    
}