using UnityEngine;

public class KeyObject : MonoBehaviour {

    GameOverMenu gameOverMenu;

	// Use this for initialization
	void Start () {
        gameOverMenu = GameObject.Find("GameOverCanvas").GetComponent<GameOverMenu>();
	}

    /// <summary>
    /// Check for player collision
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            StartCoroutine(gameOverMenu.Win());
            
        }
    }
}
