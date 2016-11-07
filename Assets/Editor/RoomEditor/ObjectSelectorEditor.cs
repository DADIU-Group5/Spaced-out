using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectSelector),true)]
public class ObjectSelectorEditor : Editor {

    ObjectSelector OS;

    enum State { unlocked, locked}
    State state;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        OS = (ObjectSelector)target;
        GetState();
        switch (state)
        {
            case State.unlocked:
                UnlockedInspector();
                break;
            case State.locked:
                LockedInspector();
                break;
            default:
                break;
        }
    }

    void UnlockedInspector()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Next Visual"))
        {
            OS.ShowNext();
        }
        if (GUILayout.Button("Prev Visual"))
        {
            OS.ShowPrev();
        }
        GUILayout.EndHorizontal();
        if (OS.gameObject.GetComponent<Renderer>().enabled == false) {
            if (GUILayout.Button("LOCK"))
            {
                OS.LockObject();
            }
        }
        if (GUILayout.Button("Show Random"))
        {
            OS.ShowRandom();
        }
        if (GUILayout.Button("Show Default"))
        {
            OS.ShowDefualt();
        }
    }

    void LockedInspector()
    {
        EditorGUILayout.LabelField("Locked as:", OS.lockedAs.name);
        if (GUILayout.Button("UNLOCK"))
        {
            OS.UnlockObject();
        }
    }

    void GetState()
    {
        if (OS.lockObject)
        {
            state = State.locked;
        }
        else
        {
            state = State.unlocked;
        }
    }
}