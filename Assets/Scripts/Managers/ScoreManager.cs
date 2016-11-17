using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>, Observer
{
    //since we often have multiple instances of scoremanager, 
    // even if one is (supposed to be) destroyed, 
    //these individual instances becomes a problem
    //when other scripts call functions, like AddComics,
    //so only 1 instance got the update. 
    //so I made the variables below static, to be shared for every instance.
    public static int totalComics;
    public static int comicsCollected;
    public static bool hasDied;

    protected override void Awake()
    {
        // will be increased on start
        base.Awake();
        totalComics = 0;
        comicsCollected = 0;
        hasDied = false;
        Subject.instance.AddObserver(this);
    }

    // add comic to level
    public void AddComics()
    {
        totalComics++;
        Debug.Log("totalComics: " + totalComics);
    }

    // comic collected
    public void ComicCollected()
    {
        comicsCollected++;
    }

    // Reset Values before next level starts
    public void ResetValues()
    {
        totalComics = 0;
        comicsCollected = 0;
        hasDied = false;
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                int level = GenerationDataManager.instance.GetCurrentLevel();
                ProgressManager.instance.SetMedal(level, ProgressManager.medalCompleted);
                if (!hasDied)
                    ProgressManager.instance.SetMedal(level, ProgressManager.medalNoDeaths);
                if (comicsCollected == totalComics)
                    ProgressManager.instance.SetMedal(level, ProgressManager.medalAllComics);
                ResetValues();
                break;
            case EventName.PlayerDead:
                hasDied = true;
                break;
            default:
                break;
        }
    }


}
