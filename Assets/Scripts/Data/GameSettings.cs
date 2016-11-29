using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class GameSettings : ScriptableObject {
    [Range(0, 100)]
    public float masterVolume;
    [Range(0, 100)]
    public float musicVolume;
    [Range(0, 100)]
    public float effectsVolume;
    public bool mute;
    public bool notifications;
    public Language language;
    public bool GodMode = false;
    public bool invertedCamera;
}
