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

    public Button nextLevelBtn;

    [HideInInspector]
    public int level = 1;

    [Header("After how long does the win screen appear:", order=1)]
    [Header("If Zero, zero times passes.", order =2)]
    public float timeTilWinScreen = 0;

    public List<GameObject> goodImages;
    public List<GameObject> badImages;

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
        
        goodImages[0].SetActive(finished);
        badImages[0].SetActive(!finished);

        goodImages[1].SetActive(didntDie);
        badImages[1].SetActive(!didntDie);

        goodImages[2].SetActive(collectedComics);
        badImages[2].SetActive(!collectedComics);
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
        if (!playerIsDead && !playerWon)
        {
            //wait set amount of time...
            yield return new WaitForSeconds(timeTilWinScreen);

            //turn on all the UI elements in the objectholder...
            for (int i = 0; i < levelCompletedMenu.transform.childCount; i++)
            {
                levelCompletedMenu.transform.GetChild(i).gameObject.SetActive(true);
            }
            playerWon = true;

            SetBadges();
        }
        yield return null;
    }
}
