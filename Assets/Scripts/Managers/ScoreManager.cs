using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>, Observer
{
    public int numberOfLevels = 5;
    private int currentLevel = 1;

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                AddAchievementToLevel(currentLevel, 1);
                if (GetPlayerHasDiedThisLevel() == 0)
                    AddAchievementToLevel(currentLevel, 2);
                break;
            case EventName.PlayerDead:
                SetPlayerHasDiedThisLevel(1);
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start () {
        Subject.instance.AddObserver(this);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        //This is how you set the number of collectibles:
        SetMaxCollectiblesForLevel(currentLevel, 1);
    }

    /// <summary>
    /// Set max collectibles for level x
    /// </summary>
    public void SetMaxCollectiblesForLevel(int level, int collectibles)
    {
        PlayerPrefs.SetInt("MaxCollectiblesForLevel"+level, collectibles);
    }

    /// <summary>
    /// Get max collectibles for level x
    /// </summary>
    public int GetMaxCollectiblesForLevel(int level)
    {
        return PlayerPrefs.GetInt("MaxCollectiblesForLevel" + level);
    }

    /// <summary>
    /// You picked up a collectible.
    /// </summary>
    public void AddCollectibles(int level)
    {
        int collected = GetCollectibles(level) + 1;
        PlayerPrefs.SetInt("Level" + level + "CollectiblesCollected", collected);

        //Congratulations, you gained all of the achievements!
        if (GetCollectibles(level) >= GetMaxCollectiblesForLevel(level))
        {
            AddAchievementToLevel(level, 3);
        }
    }

    /// <summary>
    /// Current collected collectibles for level x
    /// </summary>
    public int GetCollectibles(int level)
    {
        return PlayerPrefs.GetInt("Level" + level + "CollectiblesCollected");
    }

    /// <summary>
    /// (level, achievement) saves in playerprefs (for now)
    /// </summary>
    public void AddAchievementToLevel(int level, int achievement)
    {
        PlayerPrefs.SetInt("level" + level+ "Achievement" + achievement, 1); //ex: level2Achievement1 = 1
    }

    /// <summary>
    /// (level, achievement) 0 = false, 1 = true.
    /// </summary>
    public int GetAchievementFromLevel(int level, int achievement)
    {
        return PlayerPrefs.GetInt("level" + level + "Achievement" + achievement); //ex: level2Achievement1
    }

    public void SetPlayerHasDiedThisLevel(int died)
    {
        PlayerPrefs.SetInt("PlayerHasDiedThisLevel", died);
    }

    public int GetPlayerHasDiedThisLevel()
    {
        return PlayerPrefs.GetInt("PlayerHasDiedThisLevel");
    }

}
