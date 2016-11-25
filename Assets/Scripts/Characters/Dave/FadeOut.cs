using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    public float m_Transparency = 0.5f;
    /*public Shader m_OldShader = renderer.material.shader;
    public Color m_OldColor  = renderer.material.color;*/

    // Use this for initialization
    void Update () {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Renderer>() != null)
            {
                transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                Color C = transform.GetChild(i).GetComponent<Renderer>().material.color;
               // C.albe
                C.a = m_Transparency;
                transform.GetChild(i).GetComponent<Renderer>().material.color = C;
            }
        }
    }
	
}
