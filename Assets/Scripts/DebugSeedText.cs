using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugSeedText : MonoBehaviour {

    public Text seedText;
    GenerationData.LevelData data;

	// Use this for initialization
	void Start () {
        data = GenerationDataManager.instance.GetLevelData();
        seedText.text = "Exterior seed: " + data.exteriorSeed + "\nInterior seed: " + data.interiorSeed;
    }

    void Update()
    {
        seedText.text = "Exterior seed: " + data.exteriorSeed + "\nInterior seed: " + data.interiorSeed+"\nFPS: "+(int)(1f/Time.deltaTime);
    }
	
}
