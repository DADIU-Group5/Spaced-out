using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Brain : Singleton<Brain>, Observer
{
    // TODO: Replace with events
    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    class StateTransition
    {
        readonly State CurrentState;
        readonly Command Command;

        public StateTransition(State currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }

    Dictionary<StateTransition, State> transitions;
    public State CurrentState { get; private set; }

    public Brain()
    {
        CurrentState = State.Inactive;
        transitions = new Dictionary<StateTransition, State>
            {
                { new StateTransition(State.Inactive, Command.Exit), State.Terminated },
                { new StateTransition(State.Inactive, Command.Begin), State.Active },
                { new StateTransition(State.Active, Command.End), State.Inactive },
                { new StateTransition(State.Active, Command.Pause), State.Paused },
                { new StateTransition(State.Paused, Command.End), State.Inactive },
                { new StateTransition(State.Paused, Command.Resume), State.Active }
            };
    }

    public State GetNext(Command command)
    {
        StateTransition transition = new StateTransition(CurrentState, command);
        State nextState;
        if (!transitions.TryGetValue(transition, out nextState))
            throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        return nextState;
    }

    public State MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
        {
            case EventName.OnFire:
                MoveNext(evt);
                break;
        }
    }
}



/*
    Brain p = new Brain();
    Console.WriteLine("Current State = " + p.CurrentState);
    Console.WriteLine("Command.Begin: Current State = " + p.MoveNext(Command.Begin));
    Console.WriteLine("Command.Pause: Current State = " + p.MoveNext(Command.Pause));
    Console.WriteLine("Command.End: Current State = " + p.MoveNext(Command.End));
    Console.WriteLine("Command.Exit: Current State = " + p.MoveNext(Command.Exit));
    Console.ReadLine();
  
}
     */
