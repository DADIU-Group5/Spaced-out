using UnityEngine;
using System.Collections;
using System;

public class ProgressManager : Singleton<ProgressManager> {
    public const int medalCompleted = 0;
    public const int medalAllComics = 1;
    public const int medalShots = 2;

    public Progress progress;

    public void UnlockAll()
    {
        for (int i = 0; i < progress.levels.Length; i++)
        {
            progress.levels[i].unlocked = true;
        }
    }

    // sets the tutorial as completed
    public void completeTutorial()
    {
        progress.completedTutorial = true;
        progress.levels[0].unlocked = true;
    }

    // set medal as completed for level
    public void SetMedal(int level, int medal)
    {
        if (level < 0 || level > progress.levels.Length)
        {
            throw new UnityException("No medals exists for level: " + level);
        }

        level--;
        switch(medal)
        {
            case medalCompleted:
                if (!progress.levels[level].completed) // check if completed already
                {
                    progress.levels[level].completed = true;
                    progress.stars++;
                    // unlock next level
                    if (level < progress.levels.Length - 1)
                        progress.levels[level + 1].unlocked = true;
                }
                break;
            case medalAllComics:
                if (!progress.levels[level].allComics) // check if completed already
                {
                    progress.levels[level].allComics = true;
                    progress.stars++;
                }
                break;
            case medalShots:
                if (!progress.levels[level].shotCount) // check if completed already
                {
                    progress.levels[level].shotCount = true;
                    progress.stars++;
                }
                break;
            default:
                throw new UnityException("Medal value not in range [0..2]: " + medal);
        }
    }

    public bool SetShotCount(int level, int count)
    {
        level--;
        if (count <= GenerationDataManager.instance.GetShotCount()) {
            if(progress.levels[level].shotCount == false)
            {
                progress.levels[level].shotCount = true;
                progress.levels[level].bestShotCount = count;
                progress.stars++;
                return true;
            }
            else if(count < progress.levels[level].bestShotCount)
            {
                progress.levels[level].bestShotCount = count;
                return true;
            }
        }
        return false;
    }

    // returns an array of 3 bools indicating if medals are completed
    public bool[] GetMedals(int level)
    {
        if (level < 0 || level > progress.levels.Length)
        {
            throw new UnityException("No medals exists for level: " + level);
        }

        var levelProgress = progress.levels[level]; //- 1];
        return new bool[] { levelProgress.completed, levelProgress.shotCount, levelProgress.allComics}; 
    }

    // change the amount of stars
    public void ChangeStars(int amount)
    {
        progress.stars += amount;
    }

    // get current amount of stars
    public int GetStars() {
        return progress.stars;
    }

    // reset the stars
    public void ResetStars()
    {
        progress.stars = 0;
    }

    public void resetCurrentLevel(int levelNumber)
    {
        var level = progress.levels[levelNumber];
        level.completed = false;
        if (level.completed == true)
        {
            ProgressManager.instance.ChangeStars(-1);
        }
        level.allComics = false;
        if (level.allComics == true)
        {
            ProgressManager.instance.ChangeStars(-1);
        }
        level.shotCount = false;
        if (level.shotCount == true)
        {
            ProgressManager.instance.ChangeStars(-1);
        }
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
            level.shotCount = false;
        }
        ProgressManager.instance.ResetStars();
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
        //progress.stars = 0;
        ProgressManager.instance.ResetStars();
        progress.completedTutorial = false;
    }

    // is a level unlocked
    public bool IsUnlocked(int level)
    {
        if (level < 0 || level > progress.levels.Length-1)
        {
            throw new UnityException("Invalid level: " + level);
        }

        return progress.levels[level].unlocked;//-1].unlocked; 
    }
}
