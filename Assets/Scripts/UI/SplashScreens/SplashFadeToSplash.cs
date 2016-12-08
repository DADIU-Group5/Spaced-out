using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFadeToSplash : MonoBehaviour {

    [Header("Thes next screen object(parent):")]
    public GameObject nextSplash;
    [Header("This screens image (child):")]
    public GameObject childImage;
    [Header("Time before fading")]
    public float fadeSpeed = 1f;
    [Header("Time it takes to fade")]
    public float waitToFade = 2f;

    public GameObject bg;

    [Header("Should this screen fade out?")]
    [Tooltip("This is mostly for the last screen. Unchecking this also means that the end of this screen starts the Main Scene.")]
    public bool Fade = true;

    [HideInInspector]
    public bool fadingOut;
    [HideInInspector]
    public bool fadingIn;

    void Start()
    {
        if (childImage != null)
        {
            childImage.GetComponent<RawImage>().canvasRenderer.SetAlpha(0.0f);
        }
        if (bg != null)
            bg.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void StartFading()
    {
        fadingOut = true;
        StartCoroutine(FadeInNextScreen());
    }

    //this is called initially
    public void StartFadingIn()
    {
        fadingIn = true;
        if (bg != null)
            bg.GetComponent<Image>().canvasRenderer.SetAlpha(1.0f);
        StartCoroutine(WaitForFading());     
    }

    IEnumerator WaitForFading()
    {
        yield return new WaitForSeconds(waitToFade);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        if (Fade)
        {
            fadingOut = true;
            fadingIn = false;
            yield return new WaitForSeconds(fadeSpeed);
            fadingOut = false;
            StartCoroutine(FadeInNextScreen());
        }
        else
        {
            LoadMainScene();
            yield return null;
        }
    }

        IEnumerator FadeInNextScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        if (nextSplash != null)       
            nextSplash.GetComponent<SplashFadeToSplash>().StartFadingIn();
        else
            LoadMainScene();
        if (nextSplash != null && nextSplash.GetComponent<SplashFadeToSplash>().bg != null)
            nextSplash.GetComponent<SplashFadeToSplash>().bg.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        //if the player clicks anything
        if (Input.anyKeyDown && fadingIn || Input.anyKeyDown && fadingOut)
        {
            Debug.Log("Next screen!");
            //set the current image to null//or fade out?
            if (nextSplash != null)
                childImage.GetComponent<RawImage>().canvasRenderer.SetAlpha(0.0f);
            StopAllCoroutines();
            fadeSpeed = 0;
            StartCoroutine(FadeInNextScreen());
        }

        if (fadingOut)
        {
            if (childImage != null)
            {
                childImage.GetComponent<RawImage>().CrossFadeAlpha(0, fadeSpeed, false);
            }

        } else if (fadingIn)
        {
            if (childImage != null)
            {
                childImage.GetComponent<RawImage>().CrossFadeAlpha(1.0f, fadeSpeed, false);
            }
            /*if (bg != null)
                bg.GetComponent<Image>().CrossFadeAlpha(1.0f, fadeSpeed/2, false);*/
        }
    }
}
