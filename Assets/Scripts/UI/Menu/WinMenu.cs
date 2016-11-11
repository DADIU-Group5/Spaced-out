﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour, Observer
{
    private bool playerIsDead = false;
    private bool playerWon = false;

    [HideInInspector]
    public ScoreManager _scoreManager;

    [HideInInspector]
    public int level = 1;

    [Header("After how long does the win screen appear:")]
    [Header("If Zero, zero times passes.")]
    public float timeTilWinScreen = 0;

    public List<GameObject> goodImages;
    public List<GameObject> badImages;

    [HideInInspector]
    public bool finished = false;
    [HideInInspector]
    public bool didntDie = false;
    [HideInInspector]
    public bool collectedAll = false;

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                StartCoroutine(Win());
                break;
            case EventName.PlayerDead:
                playerIsDead = true;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        level = PlayerPrefs.GetInt("CurrentLevel");
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        //player reset, so he hasn't died in this run yet.
        _scoreManager.SetPlayerHasDiedThisLevel(0);
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    void SetBadges()
    {
        finished = (_scoreManager.GetAchievementFromLevel(level, 1) == 1) ? true : false; //ex: level2Achievement1
        didntDie = (_scoreManager.GetAchievementFromLevel(level, 2) == 1) ? true : false; //ex: level2Achievement1
        collectedAll = (_scoreManager.GetAchievementFromLevel(level, 3) == 1) ? true : false; //ex: level2Achievement1

        goodImages[0].SetActive(finished);
        badImages[0].SetActive(!finished);

        goodImages[1].SetActive(didntDie);
        badImages[1].SetActive(!didntDie);

        goodImages[2].SetActive(collectedAll);
        badImages[2].SetActive(!collectedAll);
    }

    /// <summary>
    /// Set Win Game
    /// </summary>
    public IEnumerator Win()
    {
        if (!playerIsDead && !playerWon)
        {
            //wait set amount of time...
            yield return new WaitForSeconds(timeTilWinScreen);

            //turn on all the UI elements in the objectholder...
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            playerWon = true;
            SetBadges();

        }
        yield return null;
    }
}