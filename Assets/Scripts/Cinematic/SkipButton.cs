using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour, Observer {
    void Start()
    {
        SetLanguage();
    }

    private void SetLanguage()
    {
        switch (SettingsManager.instance.GetLanguage())
        {
            case Language.Danish:
                GetComponent<Text>().text = "Spring over";
                break;
            case Language.English:
                GetComponent<Text>().text = "Skip";
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
