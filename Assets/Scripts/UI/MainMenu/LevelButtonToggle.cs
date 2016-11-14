using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButtonToggle : MonoBehaviour
{
    public Button[] levelButtons;

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
        if (LevelProgress.instance.tutorialComplete == false)
        {
            foreach (Button btn in levelButtons)
            {
                btn.interactable = false;
            }
        }
        else
        {
            if (LevelProgress.instance.levelsComplete == 0)
            {
                //
                foreach (Button btn in levelButtons)
                {
                    btn.interactable = false;
                }
                levelButtons[0].interactable = true;

            }
            else
            {
                for (int i = 0; i <= LevelProgress.instance.levelsComplete; i++)
                {
                    //
                    levelButtons[i].interactable = true;
                }
            }
        }
    }
}
