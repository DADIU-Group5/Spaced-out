using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Progress", menuName = "Progress", order = 1)]
public class Progress : ScriptableObject
{
    public int currency = 0;
    public LevelData[] levelProgress;

    public struct LevelData
    {
        public bool completedLevel;
        public bool collectedComics;
        public bool noDeaths;
    }
}