using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CalcBounds : MonoBehaviour {

    Bounds bounds;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    // Use this for initialization
    public Bounds calc () {
        bounds = new Bounds(transform.position, Vector3.zero);
        IncludeChildren(transform);
        return bounds;
	}

    void Update()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        IncludeChildren(transform);
    }

    void IncludeChildren(Transform t)
    {
        foreach (Transform child in t)
        {
            bounds.Encapsulate(child.position);
            IncludeChildren(child);
        }
    }
}
