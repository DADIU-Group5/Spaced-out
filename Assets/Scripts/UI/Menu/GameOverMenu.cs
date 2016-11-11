using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour, Observer
{

    [Header("Set countdown times:")]
    public float timeTilGameOverScreen = 1f;
    public float timeTilReset = 5f;

    public Text ResetCountdown;
    public Text DeathCauseText;

    private bool playerIsDead = false;
    private bool playerWon = false;
    private float countingDown = 10;

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerDead:
                var payload = evt.payload;
                EventName causeOfDeath = (EventName)payload[PayloadConstants.DEATH_CAUSE];
                string deathCause = "You lost...";
                switch (causeOfDeath)
                {
                    case EventName.OnFire:
                        deathCause = "You burned to death";
                        break;
                    case EventName.Crushed:
                        deathCause = "You got crushed";
                        break;
                    case EventName.Electrocuted:
                        deathCause = "You got electricuted";
                        break;
                    case EventName.PlayerExploded:
                        deathCause = "You exploded";
                        break;
                    case EventName.FuelEmpty:
                        deathCause = "You ran out of oxygen";
                        break;
                }

                DeathCauseText.text = deathCause;

                StartCoroutine(GameOver());
                break;
            case EventName.PlayerWon:
                playerWon = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set Game Over
    /// </summary>
    public IEnumerator GameOver()
    {
        if (!playerWon && !playerIsDead) {
            Debug.Log("Gameover called");
            //wait set amount of time...
            yield return new WaitForSeconds(timeTilGameOverScreen);

            //turn on all the UI elements in the GameOverCanvas
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            countingDown = timeTilReset;
            playerIsDead = true;
        }
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ResetLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void Start()
    {
        Subject.instance.AddObserver(this);
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
