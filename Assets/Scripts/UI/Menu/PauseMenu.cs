using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Text resumeText;
    public Text restartText;
    public Text mainMenuText;
    [Tooltip("Get this from the HUD!")]
    public GameObject GodModeIcon;
    private bool godMode = false;

    //keep track of whether we are pausing:
    private bool pause = false;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    private void UpdateButtonText(Language lan)
    {
        resumeText.text = Translator.instance.Get("resume");
        mainMenuText.text = Translator.instance.Get("main menu");
        restartText.text = Translator.instance.Get("restart");
    }

    /// <summary>
    /// Enable God Mode
    /// </summary>
    public void ToggleGodMode()
    {
        var evt = new ObserverEvent(EventName.GodMode);
        Subject.instance.Notify(gameObject, evt);

        godMode = !godMode;

        if (GodModeIcon != null)
        {
            for (int i = 0; i < GodModeIcon.transform.childCount; i++)
            {
                GodModeIcon.transform.GetChild(i).gameObject.SetActive(godMode);
            }
        }
            
    }

    /// <summary>
    /// Pause/Unpause game
    /// </summary>
    public void TogglePause()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;

        var evt = new ObserverEvent(EventName.Pause);
        Subject.instance.Notify(gameObject, evt);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(pause);
        }
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ReplayLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        //remember to unpause;
        TogglePause();

        var evt = new ObserverEvent(EventName.RestartLevel);
        Subject.instance.Notify(gameObject, evt);

        //player reset, so he hasn't died in this run yet.
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu()
    {
        Debug.Log("Tring to load main menu");
        //remember to unpause;
        TogglePause();
        //SceneManager.LoadScene(levelIndex);
        SceneManager.LoadScene("Main Menu");
    }
}
