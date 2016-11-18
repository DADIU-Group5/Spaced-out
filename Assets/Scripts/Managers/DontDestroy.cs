using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : Singleton<DontDestroy>
{
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
}
