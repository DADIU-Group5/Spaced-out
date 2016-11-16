using UnityEngine;
using System.Collections;
using System;

public class GenerationDataManager : Singleton<GenerationDataManager> {

    private int level;
    public GenerationData generationData;

    // resets the generation data to default
    public void Reset()
    {
        generationData.LastGenerationTime = DateTime.Now;
        for (int i = 0; i < 5; i++)
        {
            var level = generationData.levels[i];
            level.exteriorSeed = i;
            level.interiorSeed = i;
        }

    }

    // randomize new seeds for each level
    public void GenerateSeeds()
    {
        for (int i = 0; i < generationData.levels.Length; i++)
        {
            generationData.levels[i].exteriorSeed = RandomSeed();
            generationData.levels[i].interiorSeed = RandomSeed();
        }
    }

    // set the level to be loaded by the level generator
    public void SetCurrentLevel(int level)
    {
        if (level < 1 || level > generationData.levels.Length)
        {
            throw new UnityException("Invalid level: " + level);
        }
        this.level = level;
    }

    // Return the level data for generator
	public GenerationData.LevelData GetLevelData()
    {
        if (level < 1 || level > generationData.levels.Length)
            throw new UnityException("Level not set properly. Value is " + level);
        else
            return generationData.levels[level - 1];
    }

    // creates a random seed
    private int RandomSeed()
    {
        return (int)(UnityEngine.Random.value * int.MaxValue);
    }
}
