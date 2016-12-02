using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// Add this script to a gameobject with a Text component to make it automatically translate the text 
/// </summary>
[RequireComponent(typeof(Text))]
public class UITranslation : MonoBehaviour {
    [Tooltip("The translation key")]
    public string key;
    [Tooltip("Use this to add a fixed text before the translation")]
    public string prefix = "";
    [Tooltip("Use this to add a fixed text after the translation")]
    public string suffix = "";
    private Text label;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += LanguageChanged;
        label = GetComponent<Text>();
        label.text = (prefix + Translator.instance.Get(key) + suffix).ToUpper();
    }

    // called whenever the language is changed
    private void LanguageChanged(Language language)
    {
        if (label != null)
            label.text = prefix + Translator.instance.Get(key) + suffix;
    }

    void OnDestroy()
    {
        SettingsManager.instance.onLanguageChanged -= LanguageChanged;
    }
}
