using System;
using UnityEngine;



[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 1)]
public class GameSettings : ScriptableObject {
    public float volume;
    public bool notifications;
    public Language language;
}
