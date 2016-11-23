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
    public Text LostText;

    private bool playerIsDead = false;
    private bool playerWon = false;
    private float countingDown = 10;
    private int level = 1;

    void Awake()
    {
        // It is really weird that we have to do this, even though it has been drag'n'dropped in unity..
        LostText = transform.GetChild(1).GetComponent<Text>();
    }

    void Start()
    {
        Subject.instance.AddObserver(this);
        level = GenerationDataManager.instance.GetCurrentLevel();
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerDead:
                var payload = evt.payload;
                EventName causeOfDeath = (EventName)payload[PayloadConstants.DEATH_CAUSE];
                LostText.text = Translator.instance.Get("you lost") + "...";
                string deathCause = "";
                switch (causeOfDeath)
                {
                    case EventName.OnFire:
                        deathCause = Translator.instance.Get("you burned to death");
                        break;
                    case EventName.Crushed:
                        deathCause = Translator.instance.Get("you got crushed");
                        break;
                    case EventName.Electrocuted:
                        deathCause = Translator.instance.Get("you got electrocuted");
                        break;
                    case EventName.PlayerExploded:
                        deathCause = Translator.instance.Get("you exploded");
                        break;
                    case EventName.OxygenEmpty:
                        deathCause = Translator.instance.Get("you ran out of oxygen");
                        break;
                }

                DeathCauseText.text = deathCause + "!";

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

    public void RemoveGameOverScreen()
    {
        playerIsDead = false;
        //turn on all the UI elements in the GameOverCanvas
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        var evt = new ObserverEvent(EventName.RespawnPlayer);
        Subject.instance.Notify(gameObject, evt);
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
            ResetCountdown.text = Translator.instance.Get("resetting in") + " " + Mathf.Round( countingDown ) + "...";

            //if the countdown has reached zero, reset the level
            if (countingDown <= 0)
            {
                RemoveGameOverScreen();
            }
        }
	}
}
