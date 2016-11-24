using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovieTest : MonoBehaviour {

	public void OpenScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
