using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class AnimationHandler : MonoBehaviour {

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLaunchMode(bool launchMode)
    {
        animator.SetBool("Launch Mode", launchMode);
    }
}
