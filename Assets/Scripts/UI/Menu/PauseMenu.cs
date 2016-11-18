using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //keep track of whether we are pausing:
    private bool pause = false;

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
    public void LoadMainMenu(int levelIndex)
    {
        //remember to unpause;
        TogglePause();
        SceneManager.LoadScene(levelIndex);
    }
}
