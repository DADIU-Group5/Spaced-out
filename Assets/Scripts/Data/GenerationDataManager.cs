﻿using UnityEngine;
using System.Collections;
using System;

public class GenerationDataManager : Singleton<GenerationDataManager> {

    private int level = 1;
    [SerializeField]
    private GenerationData generationData;

    void Start()
    {
        if (PlayerPrefs.GetInt("PlayedBefore") == 0)
        {
            Debug.Log("randomizes");
            RandomizeSeeds();
            PlayerPrefs.SetInt("PlayedBefore", 1);
        }
    }

    /// <summary>
    /// resets the generation data to default
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < 5; i++)
        {
            var level = generationData.levels[i];
            level.exteriorSeed = i;
            level.interiorSeed = i;
        }
    }

    /// <summary>
    /// Randomizes all seeds for all levels
    /// </summary>
    public void RandomizeSeeds()
    {
        UnityEngine.Random.InitState(DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond);
        ProgressManager.instance.Reset();
        for (int i = 0; i < generationData.levels.Length; i++)
        {
            generationData.levels[i].exteriorSeed = RandomSeed();
            generationData.levels[i].interiorSeed = RandomSeed();
        }
    }

    /// <summary>
    /// Sets the current level for the level generator
    /// </summary>
    /// <param name="level">The level. 0 = tutorial</param>
    /// <param name="randomizeSeeds">Should the interior seeds be randomized</param>
    public void SetCurrentLevel(int level, bool randomizeSeeds = true)
    {
        if (level < 0 || level > generationData.levels.Length)
        {
            throw new UnityException("Invalid level: " + level);
        }

        this.level = level;
        if (randomizeSeeds && level > 0)
            generationData.levels[level-1].interiorSeed = RandomSeed();
    }

    /// <summary>
    /// Returns the current level
    /// </summary>
    /// <returns>Level</returns>
    public int GetCurrentLevel()
    {
        return level;
    }

    public int GetShotCount()
    {
        return generationData.shotCountPerLevel;
    }

    /// <summary>
    /// Return the level data used by the level generator
    /// </summary>
    /// <returns>Object containing data needed for generation</returns>
    public GenerationData.LevelData GetLevelData()
    {
        if (level < 1 || level > generationData.levels.Length)
            throw new UnityException("Level not set properly. Value is " + level);
        else
            return generationData.levels[level - 1];
    }

    private int RandomSeed()
    {
        // creates a random int value
        return UnityEngine.Random.Range(0, int.MaxValue);
    }

    public float GetHazardPercentForCurrentLevel()
    {
        return generationData.levels[level - 1].hazardPercent;
    }
}
