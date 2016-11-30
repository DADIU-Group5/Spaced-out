using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuLevelSelect : MonoBehaviour {

    public Button generateLevelButton;
    public Button[] levelButtons;
    [System.Serializable]
    public class achievementObjects
    {
        public List<GameObject> objects;
    }
    public List<achievementObjects> achievements = new List<achievementObjects>();

    public void UnlockAll()
    {
        ProgressManager.instance.UnlockAll();
        OnEnable();
    }

    void OnEnable()
    {
        generateLevelButton.interactable = ProgressManager.instance.GetCurrency() >= 15;
        // enable / disable level buttons
        for (int i = 0; i < levelButtons.Length; i++)
        {
            //if (i != 0)
            if (i < ProgressManager.instance.progress.levels.Length)
                levelButtons[i].interactable = ProgressManager.instance.IsUnlocked(i);

            if (i < ProgressManager.instance.progress.levels.Length)
            {
                bool[] medals = ProgressManager.instance.GetMedals(i);

                for (int j = 0; j < medals.Length; j++)
                {
                    achievements[i].objects[j].SetActive(medals[j]);
                }
            }
        }

        //Update achievement list
    }

    // generates new seeds for levels
    public void GenerateNewSeeds()
    {
        // can we afford it?
        if (ProgressManager.instance.GetCurrency() > 15)
        {
            GenerationDataManager.instance.RandomizeSeeds();
            ProgressManager.instance.ChangeCurrency(-15);
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
        else if (level == 1)
        {
            SceneManager.LoadScene("Intro Level 1");
            GenerationDataManager.instance.SetCurrentLevel(level);
        }
        else
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
    }
}
