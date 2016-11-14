﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour {

    public int[] levelLengths = new int[5];

    public bool overrideSeed = false;

    public int extSeed = 10;
    public int intSeed = 10;

    public int hoursFromWinToReset = 1;

    int amountOfLevels = 5;

    TimeSpan timeDifference;

    void Start()
    {
        timeDifference = new TimeSpan(hoursFromWinToReset, 0, 0);
        PlayerPrefs.SetInt("1Length", levelLengths[0]);
        PlayerPrefs.SetInt("2Length", levelLengths[1]);
        PlayerPrefs.SetInt("3Length", levelLengths[2]);
        PlayerPrefs.SetInt("4Length", levelLengths[3]);
        PlayerPrefs.SetInt("5Length", levelLengths[4]);
        //PlayerPrefs.SetString("FinishTime", DateTime.Now.ToBinary().ToString());
        //PlayerPrefs.SetInt("FinishedGame", 1);
        CheckRemakeSeed();
    }

    /// <summary>
    /// Loads a level.
    /// </summary>
    /// <param name="level"></param>
    public void LoadLevel(int level)
    {
        PlayerPrefs.SetString("CurrentLevel", level.ToString());
        if (overrideSeed)
        {
            OverrideSeeds(extSeed, intSeed, level.ToString());
        }
        SceneManager.LoadScene("LevelGenerator");
    }

    /// <summary>
    /// Used for testing, to make sure you know which level you will get!
    /// </summary>
    /// <param name="exterior"></param>
    /// <param name="interior"></param>
    /// <param name="level"></param>
    void OverrideSeeds(int exterior, int interior, string level)
    {
        PlayerPrefs.SetInt("extSeed" + level, exterior);
        PlayerPrefs.SetInt("intSeed", interior);
    }

    void CheckRemakeSeed()
    {
        Debug.Log("As this is testing, the game will check for win everytime. Has to be changed, when the player can actually win the game.");
        if(PlayerPrefs.GetInt("FinishedGame") == 1)
        {
            string finishTime = PlayerPrefs.GetString("FinishTime");
            long temp = Convert.ToInt64(finishTime);
            DateTime old = DateTime.FromBinary(temp);
            TimeSpan timeSinceLastWin = DateTime.Now.Subtract(old);
            Debug.Log("The player last won the game "+timeSinceLastWin+" ago");
            if(timeSinceLastWin > timeDifference)
            {
                Debug.Log(timeSinceLastWin + " is greater than " + timeDifference + " Will make new seeds!");
                PlayerPrefs.SetInt("FinishedGame", 0);
                ResetSeedsForAllLevels();
            }
            else
            {
                Debug.Log(timeSinceLastWin + " is less than " + timeDifference + " Will not make new seeds!");
            }
        }
        //PlayerPrefs.SetString("FinishTime", DateTime.Now.ToBinary().ToString());
    }

    public void SetOverride(bool b)
    {
        overrideSeed = b;
    }

    public void SetInteriorSeed(string inter)
    {
        intSeed = int.Parse(inter);
    }

    public void SetExteriorSeed(string exter)
    {
        extSeed = int.Parse(exter);
    }

    public void ResetSeedsForAllLevels()
    {
        for (int i = 1; i <= amountOfLevels; i++)
        {
            PlayerPrefs.SetInt("extSeed" + i.ToString(), 0);
        }
    }

    public void GenerateNewLevel()
    {
        for (int i = 1; i <= amountOfLevels; i++)
        {
            PlayerPrefs.SetInt("extSeed" + i.ToString(), 0);
            PlayerPrefs.SetInt("level" + i + "Achievement0", 0);
            PlayerPrefs.SetInt("level" + i + "Achievement1", 0);
            PlayerPrefs.SetInt("level" + i + "Achievement2", 0);
            PlayerPrefs.SetInt("Level" + i + "CollectiblesCollected", 0);
        }
    }
}