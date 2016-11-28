using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LevelButton : MonoBehaviour {

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        StopAllCoroutines();
        float a = image.color.a;

        while()
    }

    public void FadeOut()
    {
        print("Out");
    }
}
