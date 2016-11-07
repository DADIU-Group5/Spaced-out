using UnityEngine;
using System.Collections;

public class KeyObject : MonoBehaviour {

    GameOverMenu gameOverMenu;

	// Use this for initialization
	void Start () {
        gameOverMenu = GameObject.Find("GameOverCanvas").GetComponent<GameOverMenu>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Check for player collision
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision!");
        if (other.transform.tag == "Player")
        {
            Debug.Log("Player hit!");
            StartCoroutine(gameOverMenu.Win());
            
        }
    }
}
