using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour, Observer
{

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        Subject.instance.AddObserver(this);
    }
	
	public void SetLaunchMode(bool mode)
    {
        animator.SetBool("Launch Mode", mode);
    }

    public void SetPower(float power)
    {
        animator.SetFloat("Power", power);
    }

    public void SetRagdollMode(bool ragdollMode)
    {
        animator.SetBool("Ragdoll", ragdollMode);
    }

    public void ReadyForLaunch()
    {
        animator.SetTrigger("Ready For Launch");
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerLaunchModeToggle:
                SetLaunchMode(true);
                break;
            case EventName.UpdateLaunch:
                var payload = evt.payload;
                Vector2 vec = (Vector2)payload[PayloadConstants.LAUNCH_FORCE];
                float launchForce = vec.x / vec.y;
                SetPower(launchForce);
                break;
            case EventName.PlayerLaunch:
                SetLaunchMode(false);
                break;
            case EventName.PlayerLaunchCancel:
                SetLaunchMode(false);
                break;
            default:
                break;
        }
    }
}
