using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuHeader : MonoBehaviour {

    public Text medalLabel;

    public void Start()
    {
        UpdateMedalLabel();
    }

	public void UpdateMedalLabel()
    {
        medalLabel.text = PlayerPrefs.GetInt("Medals").ToString();
    }

    public void ResetMedals()
    {
        PlayerPrefs.SetInt("Medals", 0);
    }
}
