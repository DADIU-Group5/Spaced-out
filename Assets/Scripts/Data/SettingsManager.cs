﻿using UnityEngine;

public enum Language
{
    Danish, English
}

public class SettingsManager : Singleton<SettingsManager> {
    // events
    public delegate void LanguageChangedEventHandler(Language language);
    public event LanguageChangedEventHandler onLanguageChanged;
    public delegate void SensitivityChangedEventHandler(float sensitivity);
    public event SensitivityChangedEventHandler onSensitivityChanged;

    [SerializeField]
    private Settings settings;

    public void SetMasterVolume(float volume)
    {
        settings.masterVolume = volume;
        SoundManager.instance.SetMasterVolume(volume);
    }

    public float GetMasterVolume()
    {
        return settings.masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        SoundManager.instance.SetMusicVolume(volume);
    }

    public float GetMusicVolume()
    {
        return settings.musicVolume;
    }

    public void SetEffectsVolume(float volume)
    {
        settings.effectsVolume = volume;
        SoundManager.instance.SetEffectsVolume(volume);
    }

    public float GetEffectsVolume()
    {
        return settings.effectsVolume;
    }

    public void MuteSound(bool mute)
    {
        settings.mute = mute;
        SoundManager.instance.MuteSound(mute);
    }

    public bool IsMute()
    {
        return settings.mute;
    }

    public void SetLanguage(Language language)
    {
        if (settings.language != language)
        {
            settings.language = language;
            if (onLanguageChanged != null)
                onLanguageChanged(language);
        }
    }

    public Language GetLanguage()
    {
        return settings.language;
    }

    public void SetInvertedCamera(bool isInverted)
    {
        settings.invertedCamera = isInverted;
    }

    public bool GetInvertedCamera()
    {
        return settings.invertedCamera;
    }

    public void SetSensitivity(float sensitivity)
    {
        sensitivity = Mathf.Clamp01(sensitivity);
        if (settings.sensitivity != sensitivity)
        {
            settings.sensitivity = sensitivity;
            if (onSensitivityChanged != null)
                onSensitivityChanged(sensitivity);
        }
    }

    public float GetSensitivity()
    {
        return settings.sensitivity;
    }
}