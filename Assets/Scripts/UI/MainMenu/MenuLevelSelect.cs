using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
}
