﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class TutorialWinMenu : MonoBehaviour, Observer {

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch(evt.eventName)
        {
            case EventName.PlayerWon:
                ShowMenu();
                ProgressManager.instance.completeTutorial();
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    private void ShowMenu() {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void NextLevel()
    {
        GenerationDataManager.instance.SetCurrentLevel(1);
        SceneManager.LoadScene("LevelGenerator");
    }
}
