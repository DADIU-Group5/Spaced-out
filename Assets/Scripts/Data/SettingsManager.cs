using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        LoadData();
    }

    public void SetMasterVolume(float volume)
    {
        settings.masterVolume = volume;
        SoundManager.instance.SetMasterVolume(volume);
        SaveData();
    }

    public float GetMasterVolume()
    {
        return settings.masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        settings.musicVolume = volume;
        SoundManager.instance.SetMusicVolume(volume);
        SaveData();
    }

    public float GetMusicVolume()
    {
        return settings.musicVolume;
    }

    public void SetEffectsVolume(float volume)
    {
        settings.effectsVolume = volume;
        SoundManager.instance.SetEffectsVolume(volume);
        SaveData();
    }

    public float GetEffectsVolume()
    {
        return settings.effectsVolume;
    }

    public void MuteSound(bool mute)
    {
        //Debug.LogError(mute);
        settings.mute = mute;
        SoundManager.instance.MuteSound(mute);
        SaveData();
    }

    public bool IsMute()
    {
        //Debug.LogError(settings.mute);
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
        SaveData();
    }

    public Language GetLanguage()
    {
        return settings.language;
    }

    public void SetInvertedCamera(bool isInverted)
    {
        settings.invertedCamera = isInverted;
        SaveData();
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
        SaveData();
    }

    public float GetSensitivity()
    {
        return settings.sensitivity;
    }

    void SaveData()
    {
        float[] toSave = new float[7];
        toSave[0] = GetSensitivity();
        toSave[1] = GetMasterVolume();
        toSave[2] = GetMusicVolume();
        toSave[3] = GetEffectsVolume();
        if (IsMute())
        {
            toSave[4] = 1;
        }
        else
        {
            toSave[4] = 0;
        }
        if(GetLanguage() == Language.Danish)
        {
            toSave[5] = 0;
        }
        else
        {
            toSave[5] = 1;
        }
        if (GetInvertedCamera())
        {
            toSave[6] = 1;
        }
        else
        {
            toSave[6] = 0;
        }
        FileStream file = File.Create(Application.persistentDataPath + "/Settings.gd");

        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(file, toSave);
        file.Close();

    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Settings.gd"))
        {
            float[] toLoad = new float[7];
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Settings.gd", FileMode.Open);
            toLoad = (float[])bf.Deserialize(file);
            file.Close();
            SetSensitivity(toLoad[0]);
            SetMasterVolume(toLoad[1]);
            SetMusicVolume(toLoad[2]);
            SetEffectsVolume(toLoad[3]);
            if (toLoad[4] == 1)
            {
                MuteSound(true);
            }
            else
            {
                MuteSound(false);
            }
            if (toLoad[5] == 1)
            {
                SetLanguage(Language.English);
            }
            else
            {
                SetLanguage(Language.Danish);
            }
            if (toLoad[6] == 1)
            {
                SetInvertedCamera(true);
            }
            else
            {
                SetInvertedCamera(false);
            }
        }
    }
}