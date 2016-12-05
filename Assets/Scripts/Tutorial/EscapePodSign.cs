using UnityEngine;
using System.Collections;
using System;

public class EscapePodSign : MonoBehaviour, Observer {

    public Material matDanish, matEnglish;

    private Renderer renderer;

    void Start () {
        renderer = GetComponent<Renderer>();
        SetLanguage();
    }

    private void SetLanguage()
    {
        switch (SettingsManager.instance.GetLanguage())
        {
            case Language.Danish:
                renderer.material = matDanish;
                break;
            case Language.English:
                renderer.material = matEnglish;
                break;
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.ChangeLanguage)
        {
            SetLanguage();
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
