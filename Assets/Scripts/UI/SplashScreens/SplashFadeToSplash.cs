using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashFadeToSplash : MonoBehaviour {

    public GameObject nextSplash;
    public GameObject childImage;
    public float fadeSpeed = 1f;

    public bool fadingOut;
    public bool fadingIn;

    // Use this for initialization
    void Start () {
	    
	}

    public void StartFading()
    {
        fadingOut = true;
        StartCoroutine(FadeInNextScreen());
    }

    public void StartFadingIn()
    {
        fadingIn = true;
        StartCoroutine(WaitForFading());
        
    }

    IEnumerator WaitForFading()
    {
        yield return new WaitForSeconds(fadeSpeed);
    }

        IEnumerator FadeInNextScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        nextSplash.GetComponent<SplashFadeToSplash>().StartFadingIn();
        fadingIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingOut)
        {

            //orgColor.a *= fadeSpeed;
            //gameObject.GetComponent<Image>().color = orgColor;//GetComponent<Renderer>().material.color.a * fadeSpeed;
            childImage.GetComponent<RawImage>().CrossFadeAlpha(0, fadeSpeed * 0.5f, false);
            if (childImage.GetComponent<RawImage>().color.a < 0.01)
            {
                fadingOut = false;
                childImage.SetActive(false);


            }
        } else if (fadingIn)
        {
            childImage.GetComponent<RawImage>().CrossFadeAlpha(1, fadeSpeed * 0.5f, false);

            if (childImage.GetComponent<RawImage>().color.a > 0.99)
            {
                fadingIn = false;
                //nextSplash.GetComponent<SplashFadeToSplash>().StartFadingIn();
            }
        }
    }
}
