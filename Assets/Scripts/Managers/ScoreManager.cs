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
        currentLevel = int.Parse(PlayerPrefs.GetString("CurrentLevel"));

        //This is how you set the number of collectibles:
        //SetMaxCollectiblesForLevel(currentLevel, 1);

        //Resets the collected collectibles, so you cannot get it over multiple playthroughs.
        SetMaxCollectiblesForLevel(currentLevel, 0);
        PlayerPrefs.SetInt("Level" + currentLevel + "CollectiblesCollected", 0);
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

    public void AddCollectibleToLevel()
    {
        PlayerPrefs.SetInt("MaxCollectiblesForLevel" + currentLevel, PlayerPrefs.GetInt("MaxCollectiblesForLevel" + currentLevel)+1);
    }

    /// <summary>
    /// You picked up a collectible.
    /// </summary>
    public void AddCollectibles()
    {
        int collected = GetCollectibles(currentLevel) + 1;
        PlayerPrefs.SetInt("Level" + currentLevel + "CollectiblesCollected", collected);

        //Congratulations, you gained all of the achievements!
        if (GetCollectibles(currentLevel) >= GetMaxCollectiblesForLevel(currentLevel))
        {
            AddAchievementToLevel(currentLevel, 3);
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
        string key = "level" + level + "Achievement" + achievement;

        if (!PlayerPrefs.HasKey("key") || PlayerPrefs.GetInt(key) == 0)
        {
            PlayerPrefs.SetInt("Medals", PlayerPrefs.GetInt("Medals") + 1);
            PlayerPrefs.SetInt(key, 1);
        }
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
