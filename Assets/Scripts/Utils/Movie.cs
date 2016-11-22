using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Movie : MonoBehaviour {
    [Tooltip("Scene to load once the video has completed.")]
    public string scene;
    [Tooltip("Path to the video relative to the StreamingAssets folder")]
    public string videoPath;

    void Start()
    {
        StartCoroutine(PlayVideoCoroutine());
    }

    IEnumerator PlayVideoCoroutine()
    {
        Handheld.PlayFullScreenMovie(videoPath, Color.black, FullScreenMovieControlMode.CancelOnInput);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(scene);
    }
}
