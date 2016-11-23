using UnityEngine;
using System.Collections;

public class ParticleEffectSettings : MonoBehaviour {

    public float lifetime = 1;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
	
}
