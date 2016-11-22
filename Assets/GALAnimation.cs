using UnityEngine;
using System.Collections;
using System;

public class GALAnimation : MonoBehaviour, Observer
{
    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
    }
}
