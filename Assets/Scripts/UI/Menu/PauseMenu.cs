using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //keep track of whether we are pausing:
    [HideInInspector]
    public bool pausing = false;

    /// <summary>
    /// Pause/UnpauseGame
    /// </summary>
    public void PauseGame()
    {
        pausing = !pausing;

        if (pausing)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(pausing);
        }
    }

    /// <summary>
    /// Reset level
    /// </summary>
    public void ReplayLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        //remember to unpause;
        PauseGame();
        SceneManager.LoadScene(scene.name);
    }

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu(int levelIndex)
    {
        //remember to unpause;
        PauseGame();
        SceneManager.LoadScene(levelIndex);
    }
}
