using UnityEngine;
using System.Collections;

public class EpilogueMenu : MonoBehaviour {

	public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
