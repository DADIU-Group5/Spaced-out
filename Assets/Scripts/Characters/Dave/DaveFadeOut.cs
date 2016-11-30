using UnityEngine;
using System.Collections;
using System;

public class DaveFadeOut : MonoBehaviour, Observer {

    float trans = 1;
    public Material daveMat;
    public float fadeTime = 0.5f;
    bool fading = false;
    float targetTrans = 1;
    float fadeCoeef;
    bool playerSpawned = false;

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if(evt.eventName == EventName.CameraZoomValue)
        {
            if (playerSpawned)
            {
                targetTrans = (float)evt.payload[PayloadConstants.PERCENT];
                fadeCoeef = (targetTrans - trans) / fadeTime;
                fading = true;
                StartCoroutine(Fade());
            }
        }
        if(evt.eventName == EventName.PlayerSpawned)
        {
            playerSpawned = true;
        }
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
