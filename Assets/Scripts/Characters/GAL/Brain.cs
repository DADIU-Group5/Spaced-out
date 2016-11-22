﻿using UnityEngine;
using System.Collections;


public class Brain : Singleton<Brain>, Observer
{
    private bool speaking;
    public State state;
    private ObserverEvent currentEvent;
    private ObserverEvent queueEvent;
    public int seconds = 12;
    public float chance = 0.3f;
    public float windowToPlaySound = 1.5f;
    public SubtitleManager subtitleManager;

    IEnumerator SilentState()
    {
        Debug.Log("Idle: Enter");
        while (state == State.Silent)
        {
            yield return new WaitForSeconds(seconds); // TODO: refactor this, it should be wrong

            if (Random.value < chance)
            {
                state = State.Narrative;
            }
        }
        Debug.Log("Idle: Exit");
        NextState();
    }

    IEnumerator NarrativeState()
    {
        Debug.Log("Narrative: Enter");

        SubtitleType type = SubtitleType.Narrative;

        if (Random.value < 0.5f)
            type = SubtitleType.GeneralRemarks;
        
        var sub = subtitleManager.GetRandomSubtitle(Language.English, type);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(sub.duration);

        state = State.Silent;
        Debug.Log("Narrative: Exit");
        NextState();
    }

    IEnumerator MockState()
    {
        Debug.Log("Mock: Enter");

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
            }
        }
        else
        {
            type = currentEvent.eventName.EventToSubtitleType();   
        }

        var subtitle = subtitleManager.GetRandomSubtitle(Language.English, type);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, subtitle.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, subtitle.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, subtitle.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, subtitle.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(subtitle.duration);


        state = State.Silent;
        Debug.Log("Mock: Exit");
        NextState();
    }

    void Start()
    {
        Subject.instance.AddObserver(this);
        NextState();
    }

    void NextState()
    {
        string methodName = state.ToString() + "State";
        System.Reflection.MethodInfo info =
            GetType().GetMethod(methodName,
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Instance);
        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }


    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.SwitchPressed:
                goto case EventName.PlayerDead;
            case EventName.PlayerVentilated:
                goto case EventName.PlayerDead;       // YEAH BEBE! I used goto in production code ;)
            case EventName.LowOnOxygen:
                goto case EventName.PlayerDead;
            case EventName.PlayerDead:
                if (state == State.Silent)
                {
                    currentEvent = evt;
                    state = State.Mock;
                    StopCoroutine(SilentState());
                    NextState();
                }

                break;
        }
    }
}
