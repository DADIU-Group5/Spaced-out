using UnityEngine;
using System.Collections;

public class CheckpointManager : Singleton<CheckpointManager> {

    Vector3 position;
    Vector3 rotation;

    public void SetNewCheckpoint(Vector3 pos)
    {
        position = pos;
    }

    public void SetNewCheckpointRotation(Vector3 rot)
    {
        rotation = rot;
    }

    public Vector3 GetRespawnPosition()
    {
        return position;
    }

    public Vector3 GetRespawnRotation()
    {
        return rotation;
    }
}
