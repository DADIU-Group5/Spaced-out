using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButtonToggle : MonoBehaviour
{
    public Button[] levelButtons;
    private int levelsComplete = 0;

    void Start()
    {
        ToggleLevelButtons();
    }

    void Update()
    {
        ToggleLevelButtons();
    }

    public void ToggleLevelButtons()
    {
        if (PlayerPrefs.GetInt("TutorialComplete") == 1)
        {
            for (int j = 1; j <= 5; j++)
            {
                if (PlayerPrefs.GetInt("level" + j + "Achievement" + 1) == 1)
                {
                    levelsComplete = j;
                }
                else
                {
                    levelsComplete = 0;
                }
            }

            if (levelsComplete == 0)
            {
                foreach (Button btn in levelButtons)
                {
                    btn.interactable = false;
                }
                levelButtons[0].interactable = true;

            }
            else
            {
                for (int i = 0; i <= levelsComplete; i++)
                {
                    levelButtons[i].interactable = true;
                }
            }
        }
        else
        {
            foreach (Button btn in levelButtons)
            {
                btn.interactable = false;
            }
        }        
    }
}
