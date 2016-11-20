using UnityEngine;
using System.Collections;


public class Brain : Singleton<Brain>, Observer
{
    private bool speaking;
    public State state;
    private ObserverEvent currentEvent;
    private ObserverEvent queueEvent;
    public int seconds = 5;
    public float chance = 0.5f;
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

        var sub = subtitleManager.GetRandomSubtitle(Language.English, SubtitleType.GeneralRemarks);// TODO: fix this, idiot -> currentEvent.eventName);

        var narEvt = new ObserverEvent(EventName.Narrate);
        narEvt.payload.Add(PayloadConstants.NARRATIVE_ID, sub.id);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_TEXT, sub.text);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_START, sub.start);
        narEvt.payload.Add(PayloadConstants.SUBTITLE_DURATION, sub.duration);

        Subject.instance.Notify(gameObject, narEvt);

  //      while (state == State.Narrative)
  //      {
            yield return 0;
        //      }
        state = State.Silent;
        Debug.Log("Narrative: Exit");
        NextState();
    }

    IEnumerator MockState()
    {
        Debug.Log("Mock: Enter");

        var sub = subtitleManager.GetRandomSubtitle(Language.English, SubtitleType.LowOxygen);// TODO: fix this, idiot -> currentEvent.eventName);

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


//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//public class Brain : Singleton<Brain>, Observer
//{
//    class StateTransition
//    {
//        readonly State CurrentState;
//        //readonly Command Command;

//        public StateTransition(State currentState/*, Command command*/)
//        {
//            CurrentState = currentState;
//            //Command = command;
//        }

//        public override int GetHashCode()
//        {
//            return 17 + 31 * CurrentState.GetHashCode() + 31; // * Command.GetHashCode();
//        }

//        public override bool Equals(object obj)
//        {
//            StateTransition other = obj as StateTransition;
//            return other != null && this.CurrentState == other.CurrentState; // && this.Command == other.Command;
//        }
//    }

//    Dictionary<StateTransition, ObserverEvent> transitions;
//    public State CurrentState { get; private set; }

//    public Brain()
//    {
//        CurrentState = State.Inactive;
//        transitions = new Dictionary<StateTransition, State>
//            {
//                { new StateTransition(State.Inactive, Command.Exit), State.Terminated },
//                { new StateTransition(State.Inactive, Command.Begin), State.Active },
//                { new StateTransition(State.Active, Command.End), State.Inactive }
//            };
//    }

//    public State GetNext(ObserverEvent evt)//Command command)
//    {
//        StateTransition transition = new StateTransition(CurrentState, evt);
//        State nextState;
//        if (!transitions.TryGetValue(transition, out nextState))
//            throw new Exception("Invalid transition: " + CurrentState + " -> " + evt.eventName);
//        return nextState;
//    }

//    public State MoveNext(ObserverEvent evt) //Command command)
//    {
//        CurrentState = GetNext(evt);
//        return CurrentState;
//    }

//    // Use this for initialization
//    void Start () {

//	}

//	// Update is called once per frame
//	void Update () {

//	}

//    public void OnNotify(GameObject entity, ObserverEvent evt)
//    {
//        switch(evt.eventName)
//        {
//            case EventName.OnFire:
//                MoveNext(evt);
//                break;
//        }
//    }
//}



///*
//    Brain p = new Brain();
//    Console.WriteLine("Current State = " + p.CurrentState);
//    Console.WriteLine("Command.Begin: Current State = " + p.MoveNext(Command.Begin));
//    Console.WriteLine("Command.Pause: Current State = " + p.MoveNext(Command.Pause));
//    Console.WriteLine("Command.End: Current State = " + p.MoveNext(Command.End));
//    Console.WriteLine("Command.Exit: Current State = " + p.MoveNext(Command.Exit));
//    Console.ReadLine();

//}
//     */
