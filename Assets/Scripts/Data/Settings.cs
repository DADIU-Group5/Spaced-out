using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class Settings : ScriptableObject {
    [Range(0, 100)]
    public float masterVolume;
    [Range(0, 100)]
    public float musicVolume;
    [Range(0, 100)]
    public float effectsVolume;
    public bool mute;
    public Language language;
    public bool invertedCamera;

    // called from the unity editor
    void Reset()
    {
        masterVolume = 80;
        musicVolume = 80;
        effectsVolume = 80;
        mute = false;
        language = Language.Danish;
        invertedCamera = false;
    }
}
