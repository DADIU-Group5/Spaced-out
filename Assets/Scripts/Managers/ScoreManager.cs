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

    public static int shotsFired = 0;

    protected override void Awake()
    {   
        // will be increased on start
        base.Awake();
        totalComics = 0;
        comicsCollected = 0;
        shotsFired = 0;
        hasDied = false;
        if (Subject.instance != null)
        {
            Subject.instance.AddObserver(this);
        }
    }

    void Start()
    {
        hasDied = ProgressManager.instance.GetMedals(GenerationDataManager.instance.GetCurrentLevel())[2];
    }

    // add comic to level
    public void AddComics()
    {
        totalComics++;
        //update HUD comicCounter everytime a comic is added.
        var evt = new ObserverEvent(EventName.ComicsUpdate);
        evt.payload.Add(PayloadConstants.COMICS, comicsCollected+"/"+totalComics);
        Subject.instance.Notify(gameObject, evt);
    }

    // comic collected
    public void ComicCollected()
    {
        comicsCollected++;
        var evt = new ObserverEvent(EventName.ComicsUpdate);
        evt.payload.Add(PayloadConstants.COMICS, comicsCollected + "/" + totalComics);
        Subject.instance.Notify(gameObject, evt);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                //added 1 because the system is weird
                int level = GenerationDataManager.instance.GetCurrentLevel();
                ProgressManager.instance.SetMedal(level, ProgressManager.medalCompleted);
               /* if (!hasDied)
                    ProgressManager.instance.SetMedal(level, ProgressManager.medalNoDeaths);*/

                if (comicsCollected == totalComics && totalComics != 0)
                {
                    ProgressManager.instance.SetMedal(level, ProgressManager.medalAllComics);
                }

                ProgressManager.instance.SetShotCount(level, shotsFired);
                
                break;
            case EventName.PlayerDead:
                hasDied = true;
                break;
            case EventName.PlayerLaunch:
                shotsFired++;
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
