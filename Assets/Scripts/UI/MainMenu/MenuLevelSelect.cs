using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLevelSelect : MonoBehaviour {

    public Button generateLevelButton;
    public Button[] levelButtons;
	
    void OnEnable()
    {
        generateLevelButton.interactable = ProgressManager.instance.GetCurrency() >= 15;
        // enable / disable level buttons
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = ProgressManager.instance.IsUnlocked(i + 1);
        }
    }

    // generates new seeds for levels
    public void GenerateNewSeeds()
    {
        // can we afford it?
        if (ProgressManager.instance.GetCurrency() > 15)
        {
            GenerationDataManager.instance.GenerateSeeds();
            ProgressManager.instance.ChangeCurrency(-15);
        }
    }

    // loads the level
    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
    }
}
