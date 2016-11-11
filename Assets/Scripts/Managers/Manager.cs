using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : Singleton<Manager>
{
    public string sceneToLoad = "Main Menu";

	void Start () {
        DontDestroyOnLoad(gameObject);
       // SceneManager.LoadScene(sceneToLoad);
	}
}
