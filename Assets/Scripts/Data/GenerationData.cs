using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(fileName = "Generation Data", menuName = "Generation Data")]
public class GenerationData : ScriptableObject {
    public DateTime LastGenerationTime;
    public LevelData[] levels;
    
    void Reset()
    {
        LastGenerationTime = DateTime.Now;
        levels = new LevelData[5];
        for(int i = 0; i < 5; i++)
        {
            levels[i] = new LevelData()
            {
                length = i / 2 + 4,
                exteriorSeed = i,
                interiorSeed = i
            };
        }
    }

    [Serializable]
    public struct LevelData
    {
        public int length;
        public int exteriorSeed;
        public int interiorSeed;
    }
}
