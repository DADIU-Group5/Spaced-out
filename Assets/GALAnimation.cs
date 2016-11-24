using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GALAnimation : MonoBehaviour, Observer
{
    private Animator animator;

    public enum Emotion
    {
        Idle, Happy, Sad, Angry
    }

    [HideInInspector]
    public SubtitleManager subtitleManager;

    private List<string> animationStates = new List<string> { "angryTalk", "sadTalk", "happyTalk" };

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
                bool animNum = (bool)evt.payload[PayloadConstants.START_STOP];
                if (animNum)
                {
                    animator.SetTrigger(animationStates[UnityEngine.Random.Range(0, animationStates.Count)]);
                    /*var subtitle = subtitleManager.GetRandomSubtitle(Language.English, type);
                    animator.SetTrigger(animationStates[animNum]);*/
                }
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
