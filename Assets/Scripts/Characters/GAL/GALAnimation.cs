using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class GALAnimation : MonoBehaviour, Observer
{
    private Animator animator;
    // the fake SpriteRenderer
    SpriteRenderer fakeRenderer;
    Image imageCanvas;

    public enum Emotion
    {
        Idle, Happy, Sad, Angry
    }

    [HideInInspector]
    public SubtitleManager subtitleManager;

    //add  "idleTalk",  at some point.
    private List<string> animationStates = new List<string> { "angryTalk", "sadTalk", "happyTalk" };

    // Use this for initialization
    void Awake ()
    {
        imageCanvas = GetComponent<Image>();
        fakeRenderer = this.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GetComponent<Animation>().Play();
        Debug.Log("animation playing: " + GetComponent<Animation>().isPlaying);
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
        {
            case EventName.GALAnimate:
                Debug.Log("animation playing: " + GetComponent<Animation>().isPlaying);
                bool animNum = (bool)evt.payload[PayloadConstants.START_STOP];
                if (animNum)
                {
                    animator.SetTrigger(animationStates[UnityEngine.Random.Range(0, animationStates.Count)]);
                    //find some logic for emotions, either through type, or an emotion variable in the subtitles.
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

    void Update()
    {
        // if a controller is running, set the sprite
        if (animator.runtimeAnimatorController)
        {
            imageCanvas.sprite = fakeRenderer.sprite;
        }
    }
}
