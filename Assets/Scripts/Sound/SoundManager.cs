using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour, Observer
{
    uint bankID;

    [Range(0, 100)]
    public float CurrentVolume = 75;

    // Use this for initialization
    void Start()
    {
        AkSoundEngine.LoadBank("soundbank_alpha", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
    }

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    { }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        Debug.Log(evt.eventName.ToString());
        switch (evt.eventName)
        {
            case EventName.PlayerLaunch:

                var payload = evt.payload;
                float launchForce = (float)payload[PayloadConstants.LAUNCH_SPEED];

                // add game manager class that keeps track of charges so that he can do it only once
                if(launchForce > 2000)
                    PlayEvent(SoundEventConstants.DAVE_CHARGE);

                PlayEvent(SoundEventConstants.DAVE_LAUNCH);

                break;

            case EventName.OnFire:
                Debug.Log("received on fire event");
                PlayEvent(SoundEventConstants.GAL_DAVE_ON_FIRE);
                break;

            case EventName.Electrocuted:
                PlayEvent(SoundEventConstants.GAL_DEATH_ELECTROCUTED);
                break;

            case EventName.BarrelTriggered:
                break;
            case EventName.BarrelExplosion:
                PlayEvent(SoundEventConstants.EXPLOSIVE);
                break;
            case EventName.PlayerExploded:
                PlayEvent(SoundEventConstants.GAL_HAZARDS_EXPLOSION);
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
}
