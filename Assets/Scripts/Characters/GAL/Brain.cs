using UnityEngine;
using System.Collections;


public class Brain : Singleton<Brain>, Observer
{
    public State state;
    private ObserverEvent currentEvent;
    private string queuedNarrative;
    public int seconds = 12;
    public float chance = 0.4f;
    public SubtitleManager subtitleManager;

    public bool randomRemarks = true;

    private Language language;

    void Start()
    {
        Subject.instance.AddObserver(this);

        language = SettingsManager.instance.GetLanguage();

        NextState();
    }

    IEnumerator SilentState()
    {
        while (state == State.Silent)
        {
            yield return new WaitForSeconds(seconds);

            if (randomRemarks && Random.value < chance)
            {
                state = State.GeneralRemarks;
            }
        }
        
        NextState();
    }

    IEnumerator NarrativeState()
    {
        var sub = subtitleManager.GetSubtitle(queuedNarrative, language);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(sub.duration);

        state = State.Silent;
        
        NextState();
    }

    IEnumerator GeneralRemarksState()
    {
        var sub = subtitleManager.GetRandomSubtitle(language, SubtitleType.GeneralRemarks);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(sub.duration);

        state = State.Silent;
        
        NextState();
    }

    IEnumerator MockState()
    {
        SubtitleType type = SubtitleType.Narrative;

        if (currentEvent.eventName == EventName.PlayerDead)
        {
            var deathCause = (EventName)currentEvent.payload[PayloadConstants.DEATH_CAUSE];

            switch (deathCause)
            {
                case EventName.Electrocuted:
                    type = SubtitleType.Wires;
                    break;
                case EventName.OnFire:
                    type = SubtitleType.GasLeak;
                    break;
                case EventName.OxygenEmpty:
                    type = SubtitleType.OutOfOxygen;
                    break;
                case EventName.PlayerExploded:
                    type = SubtitleType.Explosion;
                    break;
            }
        }
        else if (currentEvent.eventName == EventName.PlayerVentilated)
        {
            type = SubtitleType.Fan;
        }
        else if (currentEvent.eventName == EventName.SwitchPressed)
        {
            type = SubtitleType.SwitchPressed;
        }
        else
        {
            type = currentEvent.eventName.EventToSubtitleType();   
        }

        var subtitle = subtitleManager.GetRandomSubtitle(language, type);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, subtitle.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, subtitle.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, subtitle.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, subtitle.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(subtitle.duration);

        state = State.Silent;
        
        NextState();
    }

    void NextState()
    {
        string methodName = state.ToString() + "State";
        StartCoroutine(methodName);
        //System.Reflection.MethodInfo info =
        //    GetType().GetMethod(methodName,
        //                        System.Reflection.BindingFlags.NonPublic |
        //                        System.Reflection.BindingFlags.Instance);
        //StartCoroutine((IEnumerator)info.Invoke(this, null));
    }

    public void Narrate(string remarkId)
    {
        queuedNarrative = remarkId;
        StopCoroutine("SilentState");
        state = State.Narrative;
        NextState();
    }

    public void ToggleSilence(bool silenceGAL)
    {
        if (silenceGAL)
            Subject.instance.RemoveObserver(this);
        else
            Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.SwitchPressed:
            case EventName.PlayerVentilated:
            case EventName.LowOnOxygen:
            case EventName.PlayerDead:
                if (state == State.Silent)
                {
                    StopCoroutine("SilentState");
                    currentEvent = evt;
                    state = State.Mock;
                    NextState();
                }

                break;

            // TODO: handle all those
            //case EventName.Pause:
            //case EventName.PlayerGotKey:
            //case EventName.PlayerSpawned:
            //case EventName.PlayerWon:
            //case EventName.RespawnPlayer:
            //case EventName.RestartLevel:
            //case EventName.StartCutscene:
            //case EventName.Unpause:
            //    break;

            case EventName.ChangeLanguage:
                language = (Language)evt.payload[PayloadConstants.LANGUAGE];
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
