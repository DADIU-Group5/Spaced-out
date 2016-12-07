using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ProgressManager : Singleton<ProgressManager> {
    public const int medalCompleted = 0;
    public const int medalAllComics = 1;
    public const int medalShots = 2;

    [SerializeField]
    private Progress progress;

    void Start()
    {
        LoadData();
    }

    /// <summary>
    /// Unlocks all levels
    /// </summary>
    public void UnlockAll()
    {
        for (int i = 0; i < progress.levels.Length; i++)
        {
            progress.levels[i].unlocked = true;
        }
    }

    /// <summary>
    /// Sets the tutorial as completed
    /// </summary>
    public void completeTutorial()
    {
        progress.completedTutorial = true;
        progress.levels[0].unlocked = true;
        SaveData();
    }

    /// <summary>
    /// Tracks that a star has been obtain for a level.
    /// </summary>
    /// <param name="level">1-5</param>
    /// <param name="star">0-2</param>
    /// <returns>true if the star was not obtained beforehand</returns>
    public bool ObtainStar(int level, int star)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("Could not find level: " + level);
        }

        level--;
        switch(star)
        {
            case medalCompleted:
                if (!progress.levels[level].starComplete) // check if completed already
                {
                    progress.levels[level].starComplete = true;
                    progress.stars++;
                    // unlock next level
                    if (level < progress.levels.Length - 1)
                        progress.levels[level + 1].unlocked = true;
                    SaveData();
                    return true;
                }
                break;

            case medalAllComics:
                if (!progress.levels[level].starComics) // check if completed already
                {
                    progress.levels[level].starComics = true;
                    progress.stars++;
                    SaveData();
                    return true;
                }
                break;

            case medalShots:
                if (!progress.levels[level].starBoosts) // check if completed already
                {
                    progress.levels[level].starBoosts = true;
                    progress.stars++;
                    SaveData();
                    return true;
                }
                break;
            default:
                throw new UnityException("Star value not in range [0..2]: " + star);
        }
        SaveData();
        return false;
    }

    /// <summary>
    /// Sets the boosts count. Best boost count will be updated if this count is better. 
    /// </summary>
    /// <param name="level">1-5</param>
    /// <param name="count">Number of boosts used</param>
    /// <returns>true if the boost count has improved.</returns>
    public bool SetBoostCount(int level, int count)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("Can't  find level: " + level);
        }

        level--;
        if (count < progress.levels[level].bestBoostCount && count < GenerationDataManager.instance.GetShotCount())
        {
            progress.levels[level].bestBoostCount = count;
            SaveData();
            return true;
        }
        SaveData();
        return false;
    }

    /// <summary>
    /// returns an array of 3 bools indicating which stars are obtained
    /// </summary>
    /// <param name="level">1-5</param>
    /// <returns>srray of bools</returns>
    public bool[] GetStars(int level)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("No medals exists for level: " + level);
        }

        var levelProgress = progress.levels[level - 1];
        return new bool[] { levelProgress.starComplete, levelProgress.starComics, levelProgress.starBoosts}; 
    }

    // get total number of stars
    public int GetTotalStars() {
        return progress.stars;
    }

    // resets progress in levels to default
    public void ResetLevelProgress()
    {
        // keep same seeds but reset progress
        for (int i = 0; i < progress.levels.Length; i++)
        {
            progress.levels[i].unlocked = false;
            progress.levels[i].starComplete = false;
            progress.levels[i].starComics = false;
            progress.levels[i].starBoosts = false;
            progress.levels[i].bestBoostCount = 9999;
        }
        progress.levels[0].unlocked = true;
        SaveData();
    }

    // Resets all progress to default
    public void Reset()
    {
        progress.levels = new Progress.LevelProgress[5];
        for (int i = 0; i < 5; i++)
        {
            progress.levels[i] = new Progress.LevelProgress();
            progress.levels[i].bestBoostCount = 9999;
        }
        progress.stars = 0;
        progress.completedTutorial = false;
        SaveData();
    }

    // is a level unlocked
    public bool IsUnlocked(int level)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("Invalid level: " + level);
        }
        return progress.levels[level - 1].unlocked;
    }

    void SaveData()
    {
        int[] toSave = new int[27];
        for (int i = 0; i < 5; i++)
        {
            if (progress.levels[i].unlocked)
            {
                toSave[(i * 5) + 0] = 1;
            }
            else
            {
                toSave[(i * 5) + 0] = 0;
            }
            if (progress.levels[i].starComplete)
            {
                toSave[(i * 5) + 1] = 1;
            }
            else
            {
                toSave[(i * 5) + 1] = 0;
            }
            if (progress.levels[i].starComics)
            {
                toSave[(i * 5) + 2] = 1;
            }
            else
            {
                toSave[(i * 5) + 2] = 0;
            }
            if (progress.levels[i].starBoosts)
            {
                toSave[(i * 5) + 3] = 1;
            }
            else
            {
                toSave[(i * 5) + 3] = 0;
            }
            toSave[(i * 5) + 4] = progress.levels[i].bestBoostCount;
        }
        toSave[25] = progress.stars;
        if (progress.completedTutorial)
        {
            toSave[26] = 1;
        }
        else
        {
            toSave[26] = 0;
        }

        FileStream file = File.Create(Application.persistentDataPath + "/Progress.gd");

        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(file, toSave);
        file.Close();
    }

    void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/Progress.gd"))
        {
            int[] toLoad = new int[27];
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Progress.gd", FileMode.Open);
            toLoad = (int[])bf.Deserialize(file);
            file.Close();

            for (int i = 0; i < 5; i++)
            {
                if(toLoad[(i*5)+0] == 1)
                {
                    progress.levels[i].unlocked = true;
                }
                else
                {
                    progress.levels[i].unlocked = false;
                }

                if (toLoad[(i * 5) + 1] == 1)
                {
                    progress.levels[i].starComplete = true;
                }
                else
                {
                    progress.levels[i].starComplete = false;
                }

                if (toLoad[(i * 5) + 2] == 1)
                {
                    progress.levels[i].starComics = true;
                }
                else
                {
                    progress.levels[i].starComics = false;
                }

                if (toLoad[(i * 5) + 3] == 1)
                {
                    progress.levels[i].starBoosts = true;
                }
                else
                {
                    progress.levels[i].starBoosts = false;
                }

                progress.levels[i].bestBoostCount = toLoad[(i * 5) + 4];
            }
            progress.stars = toLoad[25];
            if (toLoad[26] == 1)
            {
                progress.completedTutorial = true;
            }
            else
            {
                progress.completedTutorial = false;
            }
        }
    }
}
