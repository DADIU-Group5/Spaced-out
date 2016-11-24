using UnityEngine;
using System.Collections;


public class Brain : Singleton<Brain>, Observer
{
    public State state;
    private ObserverEvent currentEvent;
//    private ObserverEvent queueEvent;
    public int seconds = 12;
    public float chance = 0.4f;
    //public float windowToPlaySound = 1.5f;
    public SubtitleManager subtitleManager;

    void Start()
    {
        Subject.instance.AddObserver(this);
        NextState();
    }

    IEnumerator SilentState()
    {
        Debug.Log("Silent: Enter");
        while (state == State.Silent)
        {
            yield return new WaitForSeconds(seconds);

            if (Random.value < chance)
            {
                state = State.GeneralRemarks;
            }
        }
        Debug.Log("Silent: Exit");
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

    IEnumerator GeneralRemarksState()
    {
        Debug.Log("GeneralRemark: Enter");

        var sub = subtitleManager.GetRandomSubtitle(Language.English, SubtitleType.GeneralRemarks);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return new WaitForSeconds(sub.duration);

        state = State.Silent;
        Debug.Log("GeneralRemark: Exit");
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
                    StopCoroutine("SilentState");
                    currentEvent = evt;
                    state = State.Mock;
                    NextState();
                }

                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
