using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour, Observer
{
    private bool playerIsDead = false;
    private bool playerWon = false;

    [Header("After how long does the win screen appear:")]
    [Header("If Zero, zero times passes.")]
    public float timeTilWinScreen = 0;

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
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    // Update is called once per frame
    void Update () {
	
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
    /// Set Win Game
    /// </summary>
    public IEnumerator Win()
    {
        //wait set amount of time...
        yield return new WaitForSeconds(timeTilWinScreen);

        //turn on all the UI elements in the GameOverCanvas
        //assuming the player isn't dead...
        if (!playerIsDead)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            playerWon = true;
        }
        yield return null;

    }
}
