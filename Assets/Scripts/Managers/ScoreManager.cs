using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>, Observer
{
    private int totalComics;
    private int comicsCollected;














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
        currentLevel = GenerationDataManager.instance.GetCurrentLevel();

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
}
