using UnityEngine;
using System.Collections;


public class Brain : Singleton<Brain>, Observer
{
    private bool speaking;
    public State state;
    private ObserverEvent currentEvent;
    private ObserverEvent queueEvent;
    public int seconds = 5;
    public float chance = 0.3f;
    public float windowToPlaySound = 1.5f;
    public SubtitleManager subtitleManager;

    IEnumerator SilentState()
    {
        Debug.Log("Idle: Enter");
        while (state == State.Silent)
        {
            yield return new WaitForSeconds(seconds);

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

        var sub = subtitleManager.GetRandomSubtitle(Language.English, SubtitleType.Narrative); // TODO: fix this, idiot -> currentEvent.eventName);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        yield return 0;

        state = State.Silent;
        Debug.Log("Narrative: Exit");
        NextState();
    }

    IEnumerator MockState()
    {
        Debug.Log("Mock: Enter");

        var sub = subtitleManager.GetRandomSubtitle(Language.English, SubtitleType.LowOxygen); // TODO: fix this, idiot -> currentEvent.eventName);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

        while (state == State.Mock)
        {
            yield return 0;
        }
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
            case EventName.OnFire:

                break;

            case EventName.LowOnOxygen:
                if (state == State.Silent)
                {
                    state = State.Mock;
                }

                break;
        }
    }
}
