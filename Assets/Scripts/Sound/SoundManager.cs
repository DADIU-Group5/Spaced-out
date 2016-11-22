using UnityEngine;
using System.Collections;

public class SoundManager : Singleton<SoundManager>, Observer
{
    uint bankID;

    bool chargePlaying = false;

    // TODO: hide from editor 
    [Range(0, 100)]
    public float masterVolume;

    [Range(0, 100)]
    public float musicVolume;

    [Range(0, 100)]
    public float effectsVolume;

    public bool mute = false;

    private DoorOpenCloseTrigger doorTrigger;

    // Use this for initialization
    void Start()
    {
        doorTrigger = GetComponent<DoorOpenCloseTrigger>();

        AkSoundEngine.LoadBank("soundbank_alpha", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
        AkSoundEngine.SetSwitch("galVersion", "v1", gameObject);

        var settings = SettingsManager.instance.settings;

        SetMasterVolume(settings.masterVolume);
        SetMusicVolume(settings.musicVolume);
        SetEffectsVolume(settings.effectsVolume);

        mute = settings.mute;

        MuteSound(mute);
    }

    protected override void Awake()
    {
        base.Awake();
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
            case EventName.Narrate:
                var eventName = (string)evt.payload[PayloadConstants.NARRATIVE_ID];
                PlayEvent(eventName);
                break;

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

            case EventName.Door:
                // TODO: remove
                //if((bool)evt.payload[PayloadConstants.DOOR_OPEN])
                //PlayEvent(SoundEventConstants.DOOR_OPEN);
                //AkAmbient.
                //else
                //PlayEvent(SoundEventConstants.DOOR_SHUT);
                doorTrigger.Open();

                break;

            case EventName.PlayerCharge:
                bool start = (bool)evt.payload[PayloadConstants.START_STOP];
                if (start)
                {
                    if (!chargePlaying)
                    {
                        PlayEvent(SoundEventConstants.DAVE_CHARGE);
                        chargePlaying = true;
                    }
                    float force1 = (float)evt.payload[PayloadConstants.LAUNCH_FORCE];
                    AkSoundEngine.SetRTPCValue("jetpackChargeLevel", force1);
                }
                else
                {
                    StopEvent(SoundEventConstants.DAVE_CHARGE, 0);
                    chargePlaying = false;
                }
                break;
            case EventName.Collision:
                float force = (float)evt.payload[PayloadConstants.VELOCITY];
                AkSoundEngine.SetRTPCValue("velocity", force * 10);
                if ((bool)evt.payload[PayloadConstants.COLLISION_STATIC])
                {
                    PlayEvent(SoundEventConstants.DAVE_STATIC_COLLISION);
                }
                else
                {
                    PlayEvent(SoundEventConstants.DAVE_OBJECT_COLLISION);
                }
                break;
            case EventName.PlayerVentilated:
                PlayEvent(SoundEventConstants.DAVE_VENT);
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
}
