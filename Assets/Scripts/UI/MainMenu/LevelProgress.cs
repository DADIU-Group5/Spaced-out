using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelProgress : Singleton<LevelProgress>
{

    public void TutorialComplete()
    {
        PlayerPrefs.SetInt("TutorialComplete", 1);
    }


}
