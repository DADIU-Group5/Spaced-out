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

    public GameObject winMenu;

    protected override void Awake()
    {   
        // will be increased on start
        base.Awake();
        totalComics = 0;
        comicsCollected = 0;
        shotsFired = -1;
        hasDied = false;
        if (Subject.instance != null)
        {
            Subject.instance.AddObserver(this);
        }
    }

    void Start()
    {
        hasDied = ProgressManager.instance.GetStars(GenerationDataManager.instance.GetCurrentLevel())[2];
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
                if(level == 0)
                {
                    break;
                }
                ProgressManager.instance.ObtainStar(level, ProgressManager.medalCompleted);
               /* if (!hasDied)
                    ProgressManager.instance.SetMedal(level, ProgressManager.medalNoDeaths);*/

                if (comicsCollected == totalComics && totalComics != 0)
                {
                    ProgressManager.instance.ObtainStar(level, ProgressManager.medalAllComics);
                }

                if (ProgressManager.instance.SetBoostCount(level, shotsFired))
                {
                    ProgressManager.instance.ObtainStar(level, ProgressManager.medalShots);
                    var recordEvent = new ObserverEvent(EventName.PlayerRecord);
                    Subject.instance.Notify(gameObject, recordEvent);
                }
                else
                {

                }
                
                break;
            case EventName.PlayerDead:
                hasDied = true;
                break;
                //Moved this to the hud. Not pretty but works (Frederik).
            /*case EventName.PlayerLaunch:
                shotsFired++;
                Debug.LogError("fired" + shotsFired);
                break;
            case EventName.PlayerFakeLaunched:
                shotsFired--;
                Debug.LogError("firedFaked" + shotsFired);
                break;*/
            case EventName.ComicPickup:
                ComicCollected();
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
