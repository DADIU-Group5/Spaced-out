using UnityEngine;
using System.Collections;
using System;

public class DaveFadeOut : MonoBehaviour, Observer {

    float trans = 1;
    public Material daveMat;
    public float fadeTime = 0.5f;
    float baseTime;
    bool fading = false;
    float targetTrans = 1;
    float fadeCoeef;
    bool playerSpawned = false;

    void Start()
    {
        baseTime = fadeTime;
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if(evt.eventName == EventName.PlayerFadeValue)
        {
            if (playerSpawned)
            {
                targetTrans = (float)evt.payload[PayloadConstants.PERCENT];
                if (evt.payload.ContainsKey(PayloadConstants.TIME))
                {
                    fadeTime = (float)evt.payload[PayloadConstants.TIME];
                }
                else
                {
                    fadeTime = baseTime;
                }
                fadeCoeef = (targetTrans - trans) / fadeTime;
                if (!fading)
                {
                    fading = true;
                    StartCoroutine(Fade());
                }
            }
        }
        if (evt.eventName == EventName.PlayerDead)
        {
            fadeTime = 2f;
        }
        if(evt.eventName == EventName.PlayerSpawned)
        {
            playerSpawned = true;
        }
        if(evt.eventName == EventName.PlayerLaunch)
        {
            targetTrans = 1f;
            fadeCoeef = (targetTrans - trans) / fadeTime;
            if (!fading)
            {
                fading = true;
                StartCoroutine(Fade());
            }
        }
    }

    public void RespawnThing()
    {
        playerSpawned = true;
        daveMat.SetFloat("_Transparency", 0f);
        fadeTime = 2f;
        trans = 0f;
        targetTrans = 1f;
        fadeCoeef = (targetTrans - trans) / fadeTime;
        fading = true;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (fading)
        {
            trans += fadeCoeef * Time.deltaTime;
            if(fadeCoeef > 0)
            {
                if(trans > targetTrans)
                {
                    fading = false;
                    trans = targetTrans;
                }
            }
            else
            {
                if (trans < targetTrans)
                {
                    fading = false;
                    trans = targetTrans;
                }
            }
            UpdateTrans();
            yield return new WaitForEndOfFrame();
        }
        
    }

    void UpdateTrans()
    {
        daveMat.SetFloat("_Transparency", trans);
    }

    public void OnDestroy()
    {
        daveMat.SetFloat("_Transparency", 1f);
        Subject.instance.RemoveObserver(this);
    }
}
