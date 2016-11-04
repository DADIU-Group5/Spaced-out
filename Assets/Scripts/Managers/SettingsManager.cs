using UnityEngine;
using UnityEngine.UI;

public enum Language
{
    Danish, English
}

public class SettingsManager : Singleton<SettingsManager> {
    // event
    public delegate void LanguageChangedEventHandler(Language language);
    public event LanguageChangedEventHandler onLanguageChanged;
    // settings 
    public GameSettings settings;
    
	public void SetVolume(float volume)
    {
        settings.volume = volume;
        AudioListener.volume = volume / 100f;
    }

    public void SetLanguage(Language language)
    {
        if (settings.language != language)
        {
            print("Changed language: " + language);
            settings.language = language;
            onLanguageChanged(language);
        }
    }

    public Language GetLanguage()
    {
        return settings.language;
    }
}