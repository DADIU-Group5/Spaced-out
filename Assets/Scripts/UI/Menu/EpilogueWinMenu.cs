using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EpilogueWinMenu : MonoBehaviour {

    /*[Header("After how long does the win screen appear:", order = 1)]
    [Header("If Zero, zero times passes.", order = 2)]
    public float timeTilWinScreen = 0;*/

    [HideInInspector]
    public int level = 1;

    // Use this for initialization
    void Start () {
        level = GenerationDataManager.instance.GetCurrentLevel();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Load Main Menu
    /// </summary>
    public void LoadMainMenu(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// Load Next Level
    /// </summary>
    public void LoadNextLevel()
    {
        //Make sure that the player ends the cinematic flying.
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetBool("CinematicFly", false);
        level++;
        if (level <= 5)
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
        else
        {
            SceneManager.LoadScene("Epilogue");
        }
    }
}
