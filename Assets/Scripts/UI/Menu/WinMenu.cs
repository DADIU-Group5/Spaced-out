using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour, Observer
{
    private bool playerIsDead = false;
    private bool playerWon = false;

    public GameObject levelCompletedMenu;
    public GameObject winMenu;
    public GameObject hud;
    public GameObject record;

    public Text currentLevelLabel;
    public Text boostsLabel1;
    public Text boostsLabel2;
    public Text comicsLabel1;

    public Button nextLevelBtn;

    [HideInInspector]
    public int level = 1;

    [Header("After how long does the win screen appear:", order=1)]
    [Header("If Zero, zero times passes.", order =2)]
    public float timeTilWinScreen = 0;

    public List<StarAnimation> animatedStars;
    public List<GameObject> staticStars;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        level = GenerationDataManager.instance.GetCurrentLevel();
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                StartCoroutine(Win());
                break;
            case EventName.PlayerDead:
                playerIsDead = true;
                break;
            case EventName.PlayerSpawned:
                playerIsDead = false;
                break;
            case EventName.PlayerRecord:
                GotRecord();
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    public void GotRecord()
    {
        record.SetActive(true);
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        //reset current level if we restart entirely.
        //ProgressManager.instance.resetCurrentLevel(level);
        Scene scene = SceneManager.GetActiveScene();

        var evt = new ObserverEvent(EventName.RestartLevel);
        Subject.instance.Notify(gameObject, evt);

        //player reset, so he hasn't died in this run yet.
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// Load Next Level
    /// </summary>
    public void LoadNextLevel()
    {
        level++;
        if (level <= 5)
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
        else
        {
            SceneManager.LoadScene("Epilogue");
        }
    }

    void SetStars()
    {
        bool[] medals = ProgressManager.instance.GetStars(level);
        bool finished = medals[0];
        bool didntDie = medals[1];
        bool collectedComics = medals[2];
        
        StartCoroutine(InitStar(0, medals[0]));
        StartCoroutine(InitStar(1, medals[1]));
        StartCoroutine(InitStar(2, medals[2]));
    }

    // shows or hides a star
    private IEnumerator InitStar(int index, bool show)
    {
        yield return new WaitForSeconds(0.7f + 0.2f * index);
        animatedStars[index].gameObject.SetActive(show);
        staticStars[index].SetActive(show);
        if (show)
            animatedStars[index].StartAnimation();
    }

    public void ShowNextWinMenu()
    {
            levelCompletedMenu.SetActive(false);
            winMenu.SetActive(true);
    }

    public void SetScore()
    {
        boostsLabel1.text = ScoreManager.shotsFired.ToString().Replace("0", "O");
        boostsLabel2.text = ScoreManager.shotsFired.ToString().Replace("0", "O");
    }

    public void SetComics(string comicText)
    {
        comicsLabel1.text = comicText.Replace("0", "O");
    }

    /// <summary>
    /// Set Win Game
    /// </summary>
    public IEnumerator Win()
    {
        SetScore();
        if (!playerIsDead && !playerWon)
        {
            //wait set amount of time...
            yield return new WaitForSeconds(timeTilWinScreen);
            hud.SetActive(false);
            levelCompletedMenu.SetActive(true);
            int currentlevel = GenerationDataManager.instance.GetCurrentLevel();
            currentLevelLabel.text = currentlevel.ToString().Replace("0", "O");
            playerWon = true;

            SetStars();
        }
        yield return null;
    }
}
