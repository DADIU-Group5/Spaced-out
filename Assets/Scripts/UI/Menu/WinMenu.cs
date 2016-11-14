using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour, Observer
{
    private bool playerIsDead = false;
    private bool playerWon = false;

    public Button nextLevelBtn;

    [HideInInspector]
    public int level = 1;

    [Header("After how long does the win screen appear:")]
    [Header("If Zero, zero times passes.")]
    public float timeTilWinScreen = 0;

    public List<GameObject> goodImages;
    public List<GameObject> badImages;

    [HideInInspector]
    public bool finished = false;
    [HideInInspector]
    public bool didntDie = false;
    [HideInInspector]
    public bool collectedAll = false;

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                StartCoroutine(Win());
                break;
            case EventName.PlayerDead:
                //playerIsDead = true;
                break;
            case EventName.PlayerSpawned:
                playerIsDead = false;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
        level = PlayerPrefs.GetInt("CurrentLevel");

        if(level == 5)
        {
            // TODO: make invisible
            nextLevelBtn.enabled = false;
            Debug.Log("last level reached, need to do something about it");
            // transform the button
        }
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        //player reset, so he hasn't died in this run yet.
        ScoreManager.instance.SetPlayerHasDiedThisLevel(level);
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
        if (level < 5)
        {
            PlayerPrefs.SetInt("CurrentLevel", level);

            SceneManager.LoadScene("LevelGenerator");
        }
        else
        {
            Debug.Log("Loading main menu");
        }
    }

    void SetBadges()
    {
        finished = (ScoreManager.instance.GetAchievementFromLevel(level, 1) == 1) ? true : false; //ex: level2Achievement1
        didntDie = (ScoreManager.instance.GetAchievementFromLevel(level, 2) == 1) ? true : false; //ex: level2Achievement1
        collectedAll = (ScoreManager.instance.GetAchievementFromLevel(level, 3) == 1) ? true : false; //ex: level2Achievement1

        goodImages[0].SetActive(finished);
        badImages[0].SetActive(!finished);

        goodImages[1].SetActive(didntDie);
        badImages[1].SetActive(!didntDie);

        goodImages[2].SetActive(collectedAll);
        badImages[2].SetActive(!collectedAll);
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
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            playerWon = true;
            SetBadges();

            //PlayerPrefs.SetString("FinishTime", DateTime.Now.ToBinary().ToString());
            //PlayerPrefs.SetInt("FinishedGame", 1);
        }
        yield return null;
    }
}
