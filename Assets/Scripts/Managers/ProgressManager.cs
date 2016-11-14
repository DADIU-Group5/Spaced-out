using UnityEngine;
using System.Collections;

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

    // set new seeds for levels
    public void SetNewLevelSeeds(int[] seeds)
    {
        // check for correct number of seeds
        if (seeds.Length != progress.levels.Length)
        {
            throw new UnityException("Number of seeds doesn't match number of levels");
        }

        progress.SetSeeds(seeds);
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
}
