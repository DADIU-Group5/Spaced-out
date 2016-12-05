using UnityEngine;
using System.Collections;

public class LevelButtonAnimation : MonoBehaviour {

    private RectTransform transform;
    private AnimationCurve animation = AnimationCurve.EaseInOut(0, 1, 1f, 1.15f);
    private AnimationCurve translation = AnimationCurve.EaseInOut(0, 1, 1f, 1.15f);
    private float duration = 0.2f;

    void Start()
    {
        transform = GetComponent<RectTransform>();
    }

    // scales the buttons up or down based if they're in focus
    public void SetFocus(bool inFocus)
    {
        StopAllCoroutines();
        if (inFocus)
            StartCoroutine(ScaleUp());
        else
            StartCoroutine(ScaleDown());
    }

    private IEnumerator ScaleUp()
    {
        print("Scale Up");
        float t = 0;
        float scale;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            scale = animation.Evaluate(t);

            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }

    private IEnumerator ScaleDown()
    {
        print("Scale Down");
        float t = 0;
        float scale;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            scale = animation.Evaluate(1 - t);

            transform.localScale = new Vector3(scale, scale, 1);
            yield return null;
        }
    }
}
