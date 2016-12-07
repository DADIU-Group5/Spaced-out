using UnityEngine;
using System.Collections;

public class MainMenuMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AkSoundEngine.PostEvent("musicCredits", gameObject);
    }
}
