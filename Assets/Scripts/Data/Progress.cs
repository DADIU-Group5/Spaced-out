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

    // called from unity editor
    void Reset()
    {
        levelGenerationTime = DateTime.Now;
        currency = 0;
        completedTutorial = true;

        // create levels
        levels = new LevelData[5];
        for (int i = 0; i < 5; i++)
        {
            levels[i] = new LevelData()
            {
                seed = i
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