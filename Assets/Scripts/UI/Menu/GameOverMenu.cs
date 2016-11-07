using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    [Header("Set countdown times:")]
    public float timeTilGameOverScreen = 1f;
    public float timeTilReset = 1f;

    public Text ResetCountdown;

    private bool playerIsDead = false;
    private bool playerWon = false;
    private float countingDown = 10;

    /// <summary>
    /// Set Game Over
    /// </summary>
    public IEnumerator GameOver()
    {
        //wait set amount of time...
        yield return new WaitForSeconds(timeTilGameOverScreen);

        //turn on all the UI elements in the GameOverCanvas
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ResetButton")
                continue;
            if (transform.GetChild(i).gameObject.name == "WinText")
                continue;
            transform.GetChild(i).gameObject.SetActive(true);
        }
        countingDown = timeTilReset;
        playerIsDead = !playerIsDead;

    }

    /// <summary>
    /// Set Win Game
    /// </summary>
    public IEnumerator Win()
    {
        //wait set amount of time...
        //yield return new WaitForSeconds(timeTilGameOverScreen);

        //turn on all the UI elements in the GameOverCanvas
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ResetCountDownText")
                continue;
            if (transform.GetChild(i).gameObject.name == "GameOverText")
                continue;
            transform.GetChild(i).gameObject.SetActive(true);
        }
        playerWon = !playerWon;
        yield return null;

    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

	// Update is called once per frame
	void Update () {

        //if the player is dead, start counting down to level reset
	    if (playerIsDead)
        {
            countingDown -= Time.deltaTime;
            ResetCountdown.text = "Resetting in "+ Mathf.Round( countingDown ) + "..."; 
        }

        //if the countdown has reached zero, reset the level
        if (countingDown <= 0)
        {
            ResetLevel();
        }
	}
}
