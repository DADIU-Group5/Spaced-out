using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectSelector),true)]
public class ObjectSelectorEditor : Editor {

    ObjectSelector OS;

    public override void OnInspectorGUI()
    {
        OS = (ObjectSelector)target;
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
        if (GUILayout.Button("Show Random"))
        {
            OS.ShowRandom();
        }
        if (GUILayout.Button("Show Default"))
        {
            OS.ShowDefualt();
        }
    }
}