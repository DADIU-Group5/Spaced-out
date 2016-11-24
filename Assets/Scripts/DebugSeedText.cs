using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugSeedText : MonoBehaviour {

    public Text seedText;
    GenerationData.LevelData data;
    string roomName;

	// Use this for initialization
	void Start () {
        data = GenerationDataManager.instance.GetLevelData();
        seedText.text = "Exterior seed: " + data.exteriorSeed + "\nInterior seed: " + data.interiorSeed;
    }

    void Update()
    {
        seedText.text = "Ext seed: " + data.exteriorSeed + "\nInt seed: " + data.interiorSeed+"\nFPS: "+(int)(1f/Time.deltaTime)+"\nRoom name: "+roomName;
    }

    public void EnteredRoom(string name)
    {
        roomName = name.Remove(name.Length - 7, 7);
    }
	
}
