using UnityEngine;
using System.Collections;

public class MenuShop : MonoBehaviour {

    public MenuHeader header;

	// Use this for initialization
	void Awake () {
	    if (!PlayerPrefs.HasKey("Medals"))
        {
            PlayerPrefs.SetInt("Medals", 0);
        }
	}

    public void OnBuy5Click()
    {
        PlayerPrefs.SetInt("Medals", PlayerPrefs.GetInt("Medals") + 5);
        header.UpdateMedalLabel();
    }

    public void OnBuy20Click()
    {
        PlayerPrefs.SetInt("Medals", PlayerPrefs.GetInt("Medals") + 20);
        header.UpdateMedalLabel();
    }
}
