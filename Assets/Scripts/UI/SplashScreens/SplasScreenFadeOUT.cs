using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplasScreenFadeOUT : MonoBehaviour {

    public float fadeSpeed = 1f;
    public RawImage DadiuImage;
    public GameObject NextSplash;

    private bool fadingOut = false;
    private Color orgColor;

	// Use this for initialization
	void Start () {
        orgColor = gameObject.GetComponent<Image>().color;

        StartCoroutine(FadeOut());

    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeSpeed);
        fadingOut = true;
        StartCoroutine(FadeInNextScreen());
    }

    IEnumerator FadeInNextScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        NextSplash.GetComponent<SplashFadeToSplash>().StartFadingIn();
        fadingOut = false;
    }

    // Update is called once per frame
    void Update () {
	    if (fadingOut)
        {
           
            //orgColor.a *= fadeSpeed;
            //gameObject.GetComponent<Image>().color = orgColor;//GetComponent<Renderer>().material.color.a * fadeSpeed;
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
