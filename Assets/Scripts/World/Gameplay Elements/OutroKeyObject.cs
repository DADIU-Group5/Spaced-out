using UnityEngine;
using System.Collections;

public class OutroKeyObject : MonoBehaviour {

    public OutroCutScene outro;

    void Start()
    {
        //outro.StartOutro();
    }

    /// <summary>
    /// Check for player collision
    /// </summary>
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            outro.StartOutro(other.gameObject);
        }
    }
}
