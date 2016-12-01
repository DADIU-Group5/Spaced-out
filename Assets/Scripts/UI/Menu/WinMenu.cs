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
    public GameObject adsMenu;
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

    public List<GameObject> winBadges1;
    public List<GameObject> winBadges2;

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

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        level = GenerationDataManager.instance.GetCurrentLevel();
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

    void SetBadges()
    {
        bool[] medals = ProgressManager.instance.GetMedals(level-1);
        bool finished = medals[0];
        bool didntDie = medals[1];
        bool collectedComics = medals[2];
        
        winBadges1[0].SetActive(finished);
        winBadges1[1].SetActive(didntDie);
        winBadges1[2].SetActive(collectedComics);

        winBadges2[0].SetActive(finished);
        winBadges2[1].SetActive(didntDie);
        winBadges2[2].SetActive(collectedComics);

    }

    public void ShowNextWinMenu()
    {
        if (SettingsManager.instance.GetPremium() == false)
        {
            levelCompletedMenu.SetActive(false);
            adsMenu.SetActive(true);

        }
        else
        {
            ShowLastWinMenu();
        }
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

    public void ShowLastWinMenu()
    {
        if (SettingsManager.instance.GetPremium() == false)
        {
            adsMenu.SetActive(false);
        }
        else
        {
            levelCompletedMenu.SetActive(false);
        }

        winMenu.SetActive(true);
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

            SetBadges();
        }
        yield return null;
    }
}
