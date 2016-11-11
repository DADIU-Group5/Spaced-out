using UnityEngine;
using System.Collections;

public class CollectibleElement : MonoBehaviour {

    [HideInInspector]
    public ScoreManager _scoreManager;

    private int level = 0;

    // Use this for initialization
    void Start () {
        _scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        level = PlayerPrefs.GetInt("CurrentLevel");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (_scoreManager != null)
            {
                _scoreManager.AddCollectibles(level);
            }
            else
            {
                Debug.Log("CollectibleElement couldnt find the scoremanager.");
            }
            Destroy(this);
        }
    }
}
