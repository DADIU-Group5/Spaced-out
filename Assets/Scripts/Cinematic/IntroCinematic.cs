using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour {
    public Animator animator;
    public Animator galAnimator;
    public InitialForce[] floatingObjects;
    public CameraShake shakeAstroid;
    public CameraShake shakeImpact;
    public SimpelAnimation sandwichAnim;

    // Use this for initialization
    void Start () {
        GetComponent<SimpelAnimation>().PlayAnimations(() =>
        {
            SceneManager.LoadScene("TutStage01");
        });
        Invoke("PlayRest", 10f);
        Invoke("HappyTalk", 5f);
        Invoke("SadTalk", 35f);
        Invoke("Astroid", 8.05f);
        Invoke("AstroidImpact", 14.3f);
    }

    private void Astroid()
    {
        shakeAstroid.Shake();
    }

    private void AstroidImpact()
    {
        shakeImpact.Shake();
        foreach (InitialForce floatingObj in floatingObjects)
        {
            floatingObj.ApplyForce();
        }
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
