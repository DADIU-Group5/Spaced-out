using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelProgress : MonoBehaviour
{
    public bool tutorialComplete = false;
    public int levelsComplete = 0;
    public Button[] levels;

	void Start ()
    {
	
	}

    public void ResetLevelProgress()
    {
        if (tutorialComplete = false)
        {
            //disable all levels
        }
        else
        {
            //disable all but level 1
        }

    }

    public void UnlockNewLevel(int levelCompleted)
    {
        //bla
    }

    public void TutorialComplete()
    {
        tutorialComplete = true;
    }
}
