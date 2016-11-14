using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "Progress", order = 1)]
public class Progress : ScriptableObject
{
    public DateTime levelGenerationTime;
    public int currency;
    public bool completedTutorial;
    public LevelData[] levels;

    void Reset()
    {
        levelGenerationTime = DateTime.Now;
        currency = 0;
        completedTutorial = true;

        SetSeeds(new int[] { 0, 1, 2, 3, 4 });
    }

    // store new seeds for levels and reset progress
    public void SetSeeds(int[] seeds)
    {
        levelGenerationTime = DateTime.Now;
        levels = new LevelData[seeds.Length];
        for (int i = 0; i < seeds.Length; i++)
        {
            levels[i] = new LevelData()
            {
                seed = seeds[i]
            };
        }
        levels[0].unlocked = true;
    }

    [Serializable]
    public struct LevelData
    {
        public int seed;
        public bool unlocked;
        public bool completed;
        public bool allComics;
        public bool noDeaths;

    }
}