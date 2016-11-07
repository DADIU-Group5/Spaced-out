using UnityEngine;
using System.Collections;

public class BehindCamera : MonoBehaviour
{
    public GameObject target, pod, pitch;

    private void LateUpdate()
    {
        pod.transform.position = target.transform.position;
    }
}
