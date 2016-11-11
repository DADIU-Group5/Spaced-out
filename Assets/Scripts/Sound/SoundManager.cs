using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour, Observer
{
    uint bankID;

    [Range(0, 100)]
    public float masterVolume = 75;

    [Range(0, 100)]
    public float musicVolume = 75;

    [Range(0, 100)]
    public float effectsVolume = 75;

    public bool mute = false;

    // Use this for initialization
    void Start()
    {
        AkSoundEngine.LoadBank("soundbank_alpha", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
        AkSoundEngine.SetSwitch("galVersion", "v1", gameObject);
        
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetEffectsVolume(effectsVolume);
    }

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    private class Waiter
    {
        public IEnumerable Wait()
        {
            yield return new WaitForSeconds(10);
        }
    }

    // Update is called once per frame
    void Update()
    { }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        //Debug.Log(evt.eventName.ToString());

        switch (evt.eventName)
        {
            case EventName.PlayerLaunch:

                var payload = evt.payload;
                float launchForce = (float)payload[PayloadConstants.LAUNCH_FORCE];
                //Debug.Log("Launch force: " + launchForce);
                // add game manager class that keeps track of charges so that he can do it only once
                //if(launchForce > 0.75)
                //    PlayEvent(SoundEventConstants.DAVE_CHARGE);
                //else
                PlayEvent(SoundEventConstants.DAVE_LAUNCH);

                //if (firstLaunch)
                //{
                //    PlayEvent(SoundEventConstants.DAVE_FIRST_LAUNCH);
                //    firstLaunch = false;
                //}

                break;

            case EventName.OnFire:
                PlayEvent(SoundEventConstants.DAVE_CATCH_FIRE);
                break;

            case EventName.Electrocuted:
                PlayEvent(SoundEventConstants.DAVE_ELECTROCUTE);
                break;

            case EventName.BarrelTriggered:
                PlayEvent(SoundEventConstants.EXPLOSIVE);
                break;
            case EventName.BarrelExplosion:

                break;
            case EventName.PlayerDead:
                var deathCause = (EventName)evt.payload[PayloadConstants.DEATH_CAUSE];
                switch (deathCause)
                {
                    case EventName.Electrocuted:
                        PlayEvent(SoundEventConstants.GAL_DEATH_ELECTROCUTED);
                        break;
                    case EventName.OnFire:
                        StopEvent(SoundEventConstants.DAVE_CATCH_FIRE, 0);
                        PlayEvent(SoundEventConstants.GAL_DAVE_ON_FIRE);
                        break;
                    case EventName.Crushed:
                        //PlayEvent(SoundEventConstants.gal);
                        break;
                    case EventName.PlayerExploded:
                        PlayEvent(SoundEventConstants.GAL_HAZARDS_EXPLOSION);
                        break;
                }

                break;
        }
    }

    private void PlayEvent(string eventName)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
    }

    private void PlayEvent(string eventName, float fadein)
    {
        AkSoundEngine.PostEvent(eventName, gameObject);
        PauseEvent(eventName, 0);
        ResumeEvent(eventName, fadein);
    }

    private void StopEvent(string eventName, float fadeout)
    {
        uint eventID;
        eventID = AkSoundEngine.GetIDFromString(eventName);
        int fadeoutMs = (int)fadeout * 1000;
        AkSoundEngine.ExecuteActionOnEvent(
            eventID,
            AkActionOnEventType.AkActionOnEventType_Stop,
            gameObject, fadeoutMs,
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
        AkSoundEngine.SetRTPCValue("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("EffectsVolume", volume);
    }

    // maybe this is plain stupid
    public void ToggleMute()
    {
        mute = !mute;

        if(!mute)
            Subject.instance.AddObserver(this);
        else
            Subject.instance.RemoveObserver(this);
    }
}
