using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplasScreenFadeOUT : MonoBehaviour {

    public float waitToFade = 2f;
    public float fadeSpeed = 1f;
    public RawImage DadiuImage;
    public GameObject NextSplash;

    [HideInInspector]
    public bool fadingOut = false;
    [HideInInspector]
    public bool fadingIn = false;


    void Start () {
        StartCoroutine(WaitBeforeFading());
    }

    IEnumerator WaitBeforeFading()
    {
        fadingIn = true;
        yield return new WaitForSeconds(waitToFade);
        fadingOut = true;
        StartCoroutine(FadeInNextScreen());
    }

    IEnumerator FadeInNextScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        NextSplash.GetComponent<SplashFadeToSplash>().StartFadingIn();
        fadingOut = false;
        gameObject.SetActive(false);
    }

    void Update () {
        if (Input.anyKeyDown && fadingIn || Input.anyKeyDown && fadingOut)
        {
            Debug.Log("Next screen!");
            //set the current image to null//or fade out?
            DadiuImage.GetComponent<RawImage>().canvasRenderer.SetAlpha(0.0f);
            gameObject.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
            StopAllCoroutines();
            fadeSpeed = 0;
            StartCoroutine(FadeInNextScreen());
        }

        if (fadingOut)
        {
            gameObject.GetComponent<Image>().CrossFadeAlpha(0, fadeSpeed, false);
            DadiuImage.CrossFadeAlpha(0, fadeSpeed*0.5f, false);

            if (gameObject.GetComponent<Image>().color.a < 0.01f)
            {
                fadingOut = false;
                NextSplash.GetComponent<SplashFadeToSplash>().StartFading();
            }
        }
	}
}
