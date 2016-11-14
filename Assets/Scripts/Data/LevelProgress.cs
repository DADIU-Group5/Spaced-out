using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelProgress : Singleton<LevelProgress>
{
    public bool tutorialComplete = false;
    public int levelsComplete = 0;
    //public Button[] levelButtons;

	void Start ()
    {
        ResetLevelProgress();
	}

    public void ResetLevelProgress()
    {
        levelsComplete = 0;
    }

    public void HardResetLevelProgress()
    {
        tutorialComplete = false;
        levelsComplete = 0;
    }

    public void UnlockNewLevel(int levelCompleted)
    {
        levelsComplete = levelCompleted;
    }

    public void TutorialComplete()
    {
        tutorialComplete = true;
    }
}
