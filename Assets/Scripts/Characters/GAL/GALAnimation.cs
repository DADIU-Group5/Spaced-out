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
    Image GALObject;

    private bool galVisible;

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
        GALObject = transform.parent.GetComponent<Image>();
        imageCanvas = GetComponent<Image>();
        fakeRenderer = this.GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        GetComponent<Animation>().Play();
        Subject.instance.AddObserver(this);
        galVisible = true;
    }

    void Start ()
    {
        SetGAL(false);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.GALAnimate:
                bool animNum = (bool)evt.payload[PayloadConstants.START_STOP];
                if (animNum)
                {
                    animator.SetTrigger(animationStates[UnityEngine.Random.Range(0, animationStates.Count)]);
                    //find some logic for emotions, either through type, or an emotion variable in the subtitles.
                    /*var subtitle = subtitleManager.GetRandomSubtitle(Language.English, type);
                    animator.SetTrigger(animationStates[animNum]);*/
                }
                break;
            case EventName.Narrate:
                float subStart = (float)evt.payload[PayloadConstants.SUBTITLE_START];
                float subDuration = (float)evt.payload[PayloadConstants.SUBTITLE_DURATION];

                StartCoroutine(ShowGAL(subStart, subDuration));
                break;
            case EventName.ToggleGAL:
                galVisible = (bool)evt.payload[PayloadConstants.SWITCH_ON];
                break;
        }
    }

    private void SetGAL(bool b)
    {
        // Set both to b 
        if(imageCanvas != null)
            imageCanvas.enabled = GALObject.enabled = b;
    }

    /// <summary>
    /// Handle displaying the subtitle to the screen
    /// </summary>
    public IEnumerator ShowGAL(float subStart, float subDuration)//, int emotion)
    {
        if (galVisible)
        {
            yield return new WaitForSeconds(subStart);
            SetGAL(true);

            yield return new WaitForSeconds(subDuration);
            SetGAL(false);
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
