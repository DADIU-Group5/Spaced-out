using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroCinematic : MonoBehaviour {
    public Animator animator;
    public Animator galAnimator;
    public GameObject floatingObjectsParent;
    private InitialForce[] floatingObjects;
    public CameraShake shakeAstroid;
    public CameraShake shakeImpact;
    public SimpelAnimation sandwichAnim;
    public Transform comic;

    // Use this for initialization
    void Start () {
        //soundManager.DisableSounds();
        Invoke("CinematicSound", 0.0f);
        Invoke("PlayMusic", 25.0f);
        floatingObjects = floatingObjectsParent.GetComponentsInChildren<InitialForce>();
        GetComponent<SimpelAnimation>().PlayAnimations(() =>
        {
            SceneManager.LoadScene("TutStage01");
        });
        Invoke("PlayRest", 10f);
        Invoke("HappyTalk", 5f);
        Invoke("SadTalk", 35f);
        Invoke("Astroid", 8.05f);
        Invoke("AstroidImpact", 14.0f);
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

        StartCoroutine(HideComic());
    }

    private IEnumerator HideComic()
    {
        yield return new WaitForSeconds(0.15f);

        while (comic.localScale.y > 0)
        {
            comic.localScale -= new Vector3(0, 0.4f, 0);
            yield return null;
        }
        comic.gameObject.SetActive(false);
        
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

    void PlayMusic()
    {
        SoundManager.instance.StartCinematicMusic();
    }

    void CinematicSound()
    {
        AkSoundEngine.PostEvent("cinematic1", gameObject);
    }
}
