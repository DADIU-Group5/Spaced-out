using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GALAnimation : MonoBehaviour, Observer
{
    private Animator animator;

    private List<string> animationStates = new List<string> { "angryTalk", "sadTalk" };

    // Use this for initialization
    void Awake ()
    {
        animator = GetComponent<Animator>();
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
        {
            case EventName.GALAnimate:
                bool random = (bool)evt.payload[PayloadConstants.START_STOP];
                if (random)
                {
                    animator.SetTrigger(animationStates[UnityEngine.Random.Range(0, animationStates.Count)]);
                }
                break;
        }
    }
}
