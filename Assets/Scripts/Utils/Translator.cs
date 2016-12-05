using UnityEngine;
using System.Collections.Generic;
using System;

public class Translator : Singleton<Translator>
{
    public TextAsset translations;
    private Language language;
    private Dictionary<string, string> danish;
    private Dictionary<string, string> english;

    protected override void Awake()
    {
        base.Awake();
        LoadTranslations();
        language = SettingsManager.instance.GetLanguage();
        SettingsManager.instance.onLanguageChanged += LanguageChanged;
    }

    // returns a translated string
    public string Get(string key)
    {
        if (language == Language.Danish)
        {
            if (danish.ContainsKey(key))
                return danish[key];
        }
        else
        {
            if (english.ContainsKey(key))
                return english[key];
        }
        Debug.LogWarning("Translation key not found: " + key);
        return key;
    }

    private void LanguageChanged(Language lan)
    {
        language = lan;
    }

    // initial load of all translations
    private void LoadTranslations()
    {
        danish = new Dictionary<string, string>();
        english = new Dictionary<string, string>();
        var lines = translations.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

        foreach (string line in lines)
        {
            // read the translations
            string[] vals = line.Split(',');
            if (vals.Length >= 2)
                english[vals[0]] = vals[1].Trim('"');
            if (vals.Length >= 3)
                danish[vals[0]] = vals[2].Trim('"');
        }
    }

    void OnDestroy()
    {
        SettingsManager.instance.onLanguageChanged -= LanguageChanged;
    }
}
