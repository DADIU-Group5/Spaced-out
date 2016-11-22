using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "Progress", order = 1)]
public class Progress : ScriptableObject
{
    public int currency;
    public bool completedTutorial;
    public LevelProgress[] levels;

    // called from unity editor
    void Reset()
    {
        Debug.Log("Reseting medals...");
        currency = 0;
        completedTutorial = true;

        // create levels
        levels = new LevelProgress[5];
        for (int i = 0; i < 5; i++)
        {
            levels[i] = new LevelProgress();
        }

        levels[0].unlocked = true;
    }

    [Serializable]
    public struct LevelProgress
    {
        public bool unlocked;
        public bool completed;
        public bool noDeaths;
        public bool allComics;
    }
}