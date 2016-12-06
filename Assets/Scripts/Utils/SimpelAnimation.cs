using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This component can be used to create simpel animations from a sequence of animation steps
/// </summary>
public class SimpelAnimation : MonoBehaviour {
    [Serializable]
    public class AnimationStep
    {
        [Tooltip("Curve used to control animation. 0 = beginning, 1 = end")]
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [Tooltip("The length of the animation in seconds")]
        public float duration = 3f;
        [Tooltip("Target position and rotation the animation will go towards")]
        public Transform target;
    }

    public bool playOnAwake;
    public AnimationStep[] animations;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        if (playOnAwake)
            PlayAnimations(() => { });
    }

    /// <summary>
    /// Plays all camera animations
    /// </summary>
    /// <param name="callback">callback function called when all animations have finished</param>
    public void PlayAnimations(Action callback)
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        StartCoroutine(Animate(0, callback));
    }

    // this will animate the current animation and start the next automatically
    private IEnumerator Animate(int index, Action callback)
    {
        // check if all animations are finished
        if (index >= animations.Length)
        {
            callback.Invoke();
            yield break; // stop
        }

        // init animation
        AnimationStep anim = animations[index];
        float time = 0;
        float t;

        // if no target is set, we just wait for the duration and continue to next animation
        if (anim.target == null)
        {
            yield return new WaitForSeconds(anim.duration);
            StartCoroutine(Animate(index + 1, callback));
            yield break; // stop
        }

        // if no duration is set, we just go directly to target
        if (anim.duration <= 0)
        {
            transform.position = anim.target.position;
            transform.rotation = anim.target.rotation;
        }

        // do the animation
        while (time < anim.duration)
        {
            t = anim.curve.Evaluate(time / anim.duration);
            // lerp position
            if (startPosition != anim.target.position)
                transform.position = Vector3.LerpUnclamped(startPosition, anim.target.position, t);
            // lerp rotation
            if (startRotation != anim.target.rotation)
                transform.rotation = Quaternion.LerpUnclamped(startRotation, anim.target.rotation, t);

            time += Time.deltaTime;
            yield return null; // pause for a frame
        }

        // start next animation
        startPosition = anim.target.position;
        startRotation = anim.target.rotation;
        StartCoroutine(Animate(index + 1, callback));
    }
}
