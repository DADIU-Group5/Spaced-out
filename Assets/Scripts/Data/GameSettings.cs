using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class GameSettings : ScriptableObject {
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;
    public bool mute;
    public bool notifications;
    public Language language;
}
