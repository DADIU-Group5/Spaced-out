using UnityEngine;
using System.Collections;
using System;

public class DaveFadeOut : MonoBehaviour, Observer {

    float trans = 1;
    Color col;
    public Material mat;

    // Use this for initialization
    void Start()
    {
        if(mat == null)
        {
            Destroy(this);
        }
        Subject.instance.AddObserver(this);
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if(evt.eventName == EventName.CameraZoomValue)
        {
            trans = (float)evt.payload[PayloadConstants.PERCENT];
            UpdateTrans();
        }
    }

    void UpdateTrans()
    {
        col = mat.color;
        col.a = trans;
        mat.SetFloat("_MainTexOpacity", trans);
        mat.SetColor("_Color", col);
    }
}
