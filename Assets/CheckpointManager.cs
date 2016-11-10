using UnityEngine;
using System.Collections;

public class CheckpointManager : Singleton<CheckpointManager> {

    Vector3 position = Vector3.zero;
    Vector3 rotation = Vector3.zero;

    float spawnDistance;

    public GameObject playerPrefab;
    GameObject temp;

    public void SetNewCheckpoint(Vector3 pos)
    {
        position = pos;
    }

    public void SetNewCheckpointRotation(Vector3 rot)
    {
        rotation = rot;
    }

    public void SetSpawnDistance(float f)
    {
        spawnDistance = f;
    }

    public Vector3 GetRespawnPosition()
    {
        return position;
    }

    public Vector3 GetRespawnRotation()
    {
        return rotation;
    }

    public void RespawnPlayer(GameObject go)
    {
        temp = go;
        Invoke("ActualRespawn",2f);
    }

    void ActualRespawn()
    {
        Destroy(temp);
        GameObject go = Instantiate(playerPrefab, position + (rotation * spawnDistance), Quaternion.identity) as GameObject;
        go.transform.LookAt(position + (rotation * (spawnDistance+1)), Vector3.up);
    }
}
