using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour {

    public Animator animator;
    public Animator galAnimator;

	// Use this for initialization
	void Start () {
        GetComponent<SimpelAnimation>().PlayAnimations(() =>
        {
            SceneManager.LoadScene("TutStage01");
        });
        Invoke("PlayRest", 10f);
        Invoke("HappyTalk", 5f);
        Invoke("SadTalk", 35f);
	}

    private void PlayRest()
    {
        animator.SetTrigger("Rest");
    }

    private void HappyTalk()
    {
        galAnimator.SetTrigger("happyTalk");
    }

    private void SadTalk()
    {
        galAnimator.SetTrigger("sadTalk");
        Invoke("HappyTalk", 0.5f);
    }
}
