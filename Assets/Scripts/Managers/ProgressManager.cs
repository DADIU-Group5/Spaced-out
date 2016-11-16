using UnityEngine;
using System.Collections;
using System;

public class ProgressManager : Singleton<ProgressManager> {
    public const int medalCompleted = 0;
    public const int medalAllComics = 1;
    public const int medalNoDeaths = 2;

    public Progress progress;

    // set medal as completed for level
    public void SetMedal(int level, int medal)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("No medals exists for level: " + level);
        }

        switch(medal)
        {
            case medalCompleted:
                if (!progress.levels[level].completed) // check if completed already
                {
                    progress.levels[level].completed = true;
                    progress.currency++;
                    // unlock next level
                    if (level < progress.levels.Length - 1)
                        progress.levels[level + 1].unlocked = true;
                }
                break;
            case medalAllComics:
                if (!progress.levels[level].allComics) // check if completed already
                {
                    progress.levels[level].allComics = true;
                    progress.currency++;
                }
                break;
            case medalNoDeaths:
                if (!progress.levels[level].noDeaths) // check if completed already
                {
                    progress.levels[level].noDeaths = true;
                    progress.currency++;
                }
                break;
            default:
                throw new UnityException("Medal value not in range [0..2]: " + medal);
        }
    }

    // returns an array of 3 bools indicating if medals are completed
    public bool[] GetMedals(int level)
    {
        if (level < 1 || level > progress.levels.Length)
        {
            throw new UnityException("No medals exists for level: " + level);
        }

        var levelProgress = progress.levels[level];
        return new bool[] { levelProgress.completed, levelProgress.allComics, levelProgress.noDeaths }; 
    }

    // change the currency
    public void ChangeCurrency(int amount)
    {
        progress.currency += amount;
    }

    // get current amount of currency
    public int GetCurrency() {
        return progress.currency;
    }

    // reset the currency
    public void ResetCurrency()
    {
        progress.currency = 0;
    }

    // resets progress in levels to default
    public void ResetLevelProgress()
    {
        // keep same seeds but reset progress
        for (int i = 0; i < progress.levels.Length; i++)
        {
            var level = progress.levels[i];
            level.unlocked = false;
            level.completed = false;
            level.allComics = false;
            level.noDeaths = false;
        }
        progress.levels[0].unlocked = true;
    }

    // Resets all progress to default
    public void Reset()
    {
        progress.levels = new Progress.LevelProgress[5];
        for (int i = 0; i < 5; i++)
        {
            progress.levels[i] = new Progress.LevelProgress();
        }
        progress.currency = 0;
        progress.completedTutorial = false;
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
}
