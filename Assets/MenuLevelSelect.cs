using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuLevelSelect : MonoBehaviour {

    public Button generateLevelButton;
	
    void OnEnable()
    {
        generateLevelButton.interactable = PlayerPrefs.GetInt("Medals") >= 15;
    }
}
