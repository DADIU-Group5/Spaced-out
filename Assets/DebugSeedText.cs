using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugSeedText : MonoBehaviour {

    public Text seedText;

	// Use this for initialization
	void Start () {
        seedText.text = "Exterior seed: " + PlayerPrefs.GetInt("extSeed" + PlayerPrefs.GetInt("CurrentLevel")) + "\nInterior seed: " + PlayerPrefs.GetInt("intSeed");
    }
	
}
