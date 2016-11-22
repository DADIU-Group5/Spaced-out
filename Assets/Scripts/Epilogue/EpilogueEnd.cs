using UnityEngine;
using System.Collections;

public class EpilogueEnd : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("GOOD JOB! LOADING CINEMATIC.");
            Debug.Log("JK! IT IS NOT IN THE GAME. GOING TO MAIN MENU!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
