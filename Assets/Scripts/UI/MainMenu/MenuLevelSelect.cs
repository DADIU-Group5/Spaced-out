using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class MenuLevelSelect : MonoBehaviour {

    // generates new seeds for levels
    public void GenerateNewSeeds()
    {
        ProgressManager.instance.ResetLevelProgress();
        GenerationDataManager.instance.RandomizeSeeds();
    }

    // loads the level
    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("Intro Cinematic");
            GenerationDataManager.instance.SetCurrentLevel(0);
        }
        else
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
