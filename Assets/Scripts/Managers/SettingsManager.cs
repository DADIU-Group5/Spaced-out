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

    public void SetMasterVolume(float volume)
    {
        settings.masterVolume = volume;
        SoundManager.instance.SetMasterVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        SoundManager.instance.SetMusicVolume(volume);
    }

    public void SetEffectsVolume(float volume)
    {
        settings.effectsVolume = volume;
        SoundManager.instance.SetEffectsVolume(volume);
    }

    public void MuteSound(bool mute)
    {
        settings.mute = mute;

        SoundManager.instance.MuteSound(mute);
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