using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public Text resumeText;
    public Text restartText;
    public Text mainMenuText;

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
    /// Pause/Unpause game
    /// </summary>
    public void TogglePause()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
        
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
