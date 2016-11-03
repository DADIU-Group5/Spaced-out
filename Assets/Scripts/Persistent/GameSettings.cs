using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class GameSettings : ScriptableObject {
    public float volume;
    public bool muted;
    public bool notifications;
    public Language language;
}
