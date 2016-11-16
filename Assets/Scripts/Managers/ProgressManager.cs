using UnityEngine;
using System.Collections;
using System;

public class ProgressManager : Singleton<ProgressManager> {
    private const float generationCooldown = 6 * 60;
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

    // set new seeds for levels
    public void SetNewLevelSeeds(int[] seeds)
    {
        // seeds cannot be reset yet
        float mins = TimeUntilNextLevelGeneration();
        if (mins > 0)
        {
            throw new UnityException("Cannot generate levels, please wait " + mins + " minutes.");
        }

        // check for correct number of seeds
        if (seeds.Length != progress.levels.Length)
        {
            throw new UnityException("Number of seeds doesn't match number of levels");
        }

        // change seeds for levels and reset level progress
        progress.levelGenerationTime = DateTime.Now;
        for (int i = 0; i < seeds.Length; i++)
        {
            progress.levels[i].seed = seeds[i];
        }
        ResetLevelProgress();
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

    // add currency though purchase
    public void BuyCurrency(int amount)
    {
        progress.currency += amount;
    }

    // get current amount of currency
    public int GetCurrency() {
        return progress.currency;
    }

    // gets the number of minutes since last level generation
    public float TimeUntilNextLevelGeneration()
    {
        // cooldown of level generation in minutes
        float minsLeft = (float)(DateTime.Now - progress.levelGenerationTime).TotalMinutes - generationCooldown;
        return minsLeft < 0f ? 0f : minsLeft;
    }

    // reset the medals
    public void ResetCurrency()
    {
        progress.currency = 0;
    }

    // reset progress
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
}
