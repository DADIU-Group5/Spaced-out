using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugSeedText : MonoBehaviour {

    public Text seedText;

	// Use this for initialization
	void Start () {
        var data = GenerationDataManager.instance.GetLevelData();
        seedText.text = "Exterior seed: " + data.exteriorSeed + "\nInterior seed: " + data.interiorSeed;
    }
	
}
