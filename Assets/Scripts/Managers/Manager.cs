using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
    public string sceneToLoad = "Main Menu";

	void Start () {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(sceneToLoad);
	}
}
