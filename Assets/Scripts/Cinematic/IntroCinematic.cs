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
    public Transform comic;

    // Use this for initialization
    void Start () {
        // in case player got here from the menu while playing a different level
        SoundManager.instance.StopMusic();

        Invoke("CinematicSound", 0.0f);
        Invoke("Narrative1", 2.0f);
        Invoke("Narrative2", 11.0f);
        Invoke("Remark9", 35.0f);
        Invoke("PlayMusic", 25.0f);
        floatingObjects = floatingObjectsParent.GetComponentsInChildren<InitialForce>();
        GetComponent<SimpelAnimation>().PlayAnimations(LoadScene);
        Invoke("PlayRest", 10f);
        Invoke("HappyTalk", 5f);
        Invoke("SadTalk", 35f);
        Invoke("Astroid", 8.05f);
        Invoke("AstroidImpact", 14.0f);
    }

    public void SkipIntro()
    {
        // Stop all sounds and animations that were scheduled
        CancelInvoke();

        SoundManager.instance.StopEvent("cinematic1", 0, gameObject);
        SoundManager.instance.StopEvent("narrative1", 0);
        SoundManager.instance.StopEvent("narrative2", 0);
        SoundManager.instance.StopEvent("remark9", 0);

        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("TutStage01");
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

    private void Narrative1()
    {
        Brain.instance.Narrate("narrative1");
    }

    private void Narrative2()
    {
        Brain.instance.Narrate("narrative2");
    }

    private void Remark9()
    {
        Brain.instance.Narrate("remark9");
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
