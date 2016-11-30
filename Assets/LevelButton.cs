using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public bool unlocked;
    public Text levelText;
    public Text boostsText;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    // Update all UI elements for the level button
    public void SetLevel(int level)
    {
        levelText.text = level.ToString();

        if (unlocked && level > 0)
        {
            var stars = ProgressManager.instance.GetMedals(level);
            // get stars;
            star1.SetActive(stars[0]);
            star2.SetActive(stars[1]);
            star3.SetActive(stars[2]);
        }
    }
}
