using UnityEngine;
using System.Collections;

public class StarAnimation : MonoBehaviour {

    private float duration = 0.3f;

	public void StartAnimation()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        transform.localScale = Vector3.zero;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
    }
}
