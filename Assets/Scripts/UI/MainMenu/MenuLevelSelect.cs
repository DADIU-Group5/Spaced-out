using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLevelSelect : MonoBehaviour {

    public LevelButton[] levelButtons;

    public void UnlockAll()
    {
        ProgressManager.instance.UnlockAll();
    }

    // generates new seeds for levels
    public void GenerateNewSeeds()
    {
        // can we afford it?
        if (ProgressManager.instance.GetStars() > 15)
        {
            GenerationDataManager.instance.RandomizeSeeds();
            ProgressManager.instance.ChangeStars(-15);
        }
    }

    // loads the level
    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("Intro Cinematic");
            GenerationDataManager.instance.SetTutortialLevel();
        }
        else
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
    }
}
