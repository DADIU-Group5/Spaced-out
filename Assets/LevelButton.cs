using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public int level;
    public GameObject disabledButton;
    public Text boostsText;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    void OnEnable()
    {
        // unlock/lock button
        bool unlocked = ProgressManager.instance.IsUnlocked(level);
        disabledButton.SetActive(!unlocked);
        gameObject.SetActive(unlocked);

        // update stars
        if (unlocked)
        {
            var stars = ProgressManager.instance.GetMedals(level);
            star1.SetActive(stars[0]);
            star2.SetActive(stars[1]);
            star3.SetActive(stars[2]);
        }
    }
}
