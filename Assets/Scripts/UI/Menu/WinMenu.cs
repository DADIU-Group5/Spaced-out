using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour, Observer
{
    private bool playerIsDead = false;
    private bool playerWon = false;

    [Header("After how long does the win screen appear:")]
    [Header("If Zero, zero times passes.")]
    public float timeTilWinScreen = 0;

    public List<GameObject> goodImages;
    public List<GameObject> badImages;

    [HideInInspector]
    public bool finished = true;
    [HideInInspector]
    public bool didntDie = false;
    [HideInInspector]
    public bool collectedAll = false;

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerWon:
                Debug.Log("WinMenu registers win!");
                StartCoroutine(Win());
                break;
            case EventName.PlayerDead:
                playerIsDead = true;
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    void SetBadges()
    {
        finished = true;
        Debug.Log("Setting badges: finished: " + finished + " and didntdie: " + didntDie + " and collectedAll: " + collectedAll);

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
        Debug.Log("in win coroutine...");
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

        }
        yield return null;
    }
}
