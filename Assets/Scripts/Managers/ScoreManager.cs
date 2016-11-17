using UnityEngine;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>, Observer
{
    private int totalComics;
    private int comicsCollected;
    private bool hasDied;

    protected override void Awake()
    {
        base.Awake();
        // will be increased on start
        totalComics = 0;
        comicsCollected = 0;
        hasDied = false;
        Subject.instance.AddObserver(this);
    }

    // add comic to level
    public void AddComics()
    {
        totalComics++;
    }

    // comic collected
    public void ComicCollected()
    {
        comicsCollected++;
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
                break;
            case EventName.PlayerDead:
                hasDied = true;
                break;
            default:
                break;
        }
    }


}
