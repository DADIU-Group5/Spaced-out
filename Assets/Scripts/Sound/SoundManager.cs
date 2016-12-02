using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>, Observer
{
    uint bankID;

    bool chargePlaying = false;

    private float masterVolume;
    private float musicVolume;
    private float effectsVolume;
    private bool mute = false;

    private bool musicPlaying;
    private bool atmoshericSoundPlaying;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);

        AkSoundEngine.LoadBank("soundbank_alpha", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);

        var settings = SettingsManager.instance.settings;

        SetMasterVolume(settings.masterVolume);
        SetMusicVolume(settings.musicVolume);
        SetEffectsVolume(settings.effectsVolume);

        mute = settings.mute;

        MuteSound(mute);

        SetLanguage(settings.language);

        musicPlaying = false;
        atmoshericSoundPlaying = false;
    }

    private class Waiter
    {
        public IEnumerable Wait()
        {
            yield return new WaitForSeconds(10);
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.Narrate:
                var eventName = (string)evt.payload[PayloadConstants.NARRATIVE_ID];
                PlayEvent(eventName);
                break;

            case EventName.PlayerLaunch:
                PlayEvent(SoundEventConstants.DAVE_LAUNCH, gameObject);

                chargePlaying = false;

                break;

            case EventName.OnFire:
                PlayEvent(SoundEventConstants.DAVE_CATCH_FIRE);
                Invoke("PutOutDave", 6.0f);
                break;

            case EventName.Electrocuted:
                PlayEvent(SoundEventConstants.DAVE_ELECTROCUTE);
                Invoke("StopElectrocution", 5.0f);
                break;

            case EventName.PlayerCharge:
                var jetpackState = (JetPackState)evt.payload[PayloadConstants.START_STOP];
                float force1 = (float)evt.payload[PayloadConstants.LAUNCH_FORCE];

                switch (jetpackState)
                {
                    case JetPackState.StartCharging:
                        if (!chargePlaying)
                        {
                            PlayEvent(SoundEventConstants.DAVE_CHARGE);

                            //AkSoundEngine.SetRTPCValue("jetpackChargeLevel", force1);
                            chargePlaying = true;
                        }
                        break;

                    case JetPackState.StopCharging:
                        AkSoundEngine.SetRTPCValue("jetpackChargeLevel", 0);
                        StopEvent(SoundEventConstants.DAVE_CHARGE, 0.0f);
                        chargePlaying = false;

                        break;
                }
                break;

            case EventName.LaunchPowerChanged:
                var forceVec = (Vector2)evt.payload[PayloadConstants.LAUNCH_FORCE];
                AkSoundEngine.SetRTPCValue("jetpackChargeLevel", forceVec.x * 10.0f);
                break;

            case EventName.Collision:
                float force = (float)evt.payload[PayloadConstants.VELOCITY];
                AkSoundEngine.SetRTPCValue("velocity", force * 10);
                if ((bool)evt.payload[PayloadConstants.COLLISION_STATIC])
                {
                    PlayEvent(SoundEventConstants.DAVE_STATIC_COLLISION, entity);
                }
                else
                {
                    PlayEvent(SoundEventConstants.DAVE_OBJECT_COLLISION, entity);
                }
                break;
            case EventName.PlayerVentilated:
                PlayEvent(SoundEventConstants.DAVE_VENT);
                break;

            case EventName.UIButton:
                PlayEvent((string)evt.payload[PayloadConstants.TYPE]);
                break;

            case EventName.SwitchPressed:

                if ((bool)evt.payload[PayloadConstants.SWITCH_ON])
                {
                    PlayEvent(SoundEventConstants.SWITCH_ON, entity);
                }
                else
                {
                    PlayEvent(SoundEventConstants.SWITCH_OFF, entity);
                }

                break;
            case EventName.ChangeLanguage:
                SetLanguage((Language)evt.payload[PayloadConstants.LANGUAGE]);
                break;

            case EventName.Door:
                if ((bool)evt.payload[PayloadConstants.DOOR_OPEN])
                {
                    StopEvent(SoundEventConstants.DOOR_OPEN, 0, entity);
                    AkSoundEngine.PostEvent(SoundEventConstants.DOOR_OPEN, entity);
                }
                else
                {
                    AkSoundEngine.PostEvent(SoundEventConstants.DOOR_SHUT, entity);
                }
                break;

            case EventName.StartCutscene:
                if (evt.payload.ContainsKey(PayloadConstants.START_LEVEL))
                    if ((bool)evt.payload[PayloadConstants.START_LEVEL])
                        StartMusic();
                break;
        }
    }

    private void SetLanguage(Language language)
    {
        if (language == Language.English)
            AkSoundEngine.SetState("galLanguage", "Eng");
        else
            AkSoundEngine.SetState("galLanguage", "Dan");
    }

    private void PutOutDave()
    {
        StopEvent(SoundEventConstants.DAVE_CATCH_FIRE, 0.5f);
    }

    private void StopElectrocution()
    {
        StopEvent(SoundEventConstants.DAVE_ELECTROCUTE, 0.5f);
    }

    private void PlayEvent(string eventName)
    {
        PlayEvent(eventName, gameObject);
    }

    private void PlayEvent(string eventName, GameObject entity)
    {
        AkSoundEngine.PostEvent(eventName, entity);
    }

    private void PlayEvent(string eventName, float fadein)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
        PauseEvent(eventName, 0);
        ResumeEvent(eventName, fadein);
    }

    private void StopEvent(string eventName, float fadeout)
    {
        StopEvent(eventName, fadeout, gameObject);
    }

    private void StopEvent(string eventName, float fadeout, GameObject entity)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        int fadeoutMs = (int)fadeout * 1000;
        AkSoundEngine.ExecuteActionOnEvent(
            eventID,
            AkActionOnEventType.AkActionOnEventType_Stop,
            entity, fadeoutMs,
            AkCurveInterpolation.
            AkCurveInterpolation_Sine);
    }

    private void PauseEvent(string eventName, float fadeout)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        int fadeoutMs = (int)fadeout * 1000;
        AkSoundEngine.ExecuteActionOnEvent(
            eventID,
            AkActionOnEventType.AkActionOnEventType_Pause,
            gameObject,
            fadeoutMs,
            AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    private void ResumeEvent(string eventName, float fadein)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        int fadeinMs = (int)fadein * 1000;
        AkSoundEngine.ExecuteActionOnEvent(
            eventID,
            AkActionOnEventType.AkActionOnEventType_Resume,
            gameObject,
            fadeinMs,
            AkCurveInterpolation.AkCurveInterpolation_Sine);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        AkSoundEngine.SetRTPCValue("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        AkSoundEngine.SetRTPCValue("musicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        effectsVolume = volume;
        AkSoundEngine.SetRTPCValue("effectsVolume", volume);
    }

    public void MuteSound(bool mute)
    {
        this.mute = mute;

        if (mute)
            AkSoundEngine.SetRTPCValue("masterVolume", 0.0f);
        else
            AkSoundEngine.SetRTPCValue("masterVolume", masterVolume);
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void EnableSounds()
    {
        Subject.instance.AddObserver(this);
    }

    public void DisableSounds()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void StartCinematicMusic()
    {
        musicPlaying = true;
        atmoshericSoundPlaying = true;

        AkSoundEngine.PostEvent("musicFirstPlay", gameObject);
        AkSoundEngine.PostEvent("atmosStart", gameObject);
    }

    public void StartMusic()
    {
        if (!musicPlaying)
        {
            AkSoundEngine.PostEvent(SoundEventConstants.MUSIC_MAIN_PLAY, gameObject);
            AkSoundEngine.PostEvent("atmosStart", gameObject);

            musicPlaying = true;
        }
    }

    public void StopMusic()
    {
        AkSoundEngine.PostEvent(SoundEventConstants.MUSIC_MAIN_STOP, gameObject);
    }
}
