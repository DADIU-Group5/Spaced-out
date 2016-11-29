using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SimpelAnimation>().PlayAnimations(() =>
        {
            //SceneManager.LoadScene("TutStage01");
        });
	}
}
