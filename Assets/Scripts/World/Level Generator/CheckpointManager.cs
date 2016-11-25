using UnityEngine;
using System.Collections;

public class CheckpointManager : Singleton<CheckpointManager>, Observer {

    Vector3 position = Vector3.zero;
    Vector3 rotation = Vector3.zero;
    int fuelCount = 0;

    float spawnDistance = 0;

    public GameObject playerPrefab;
    GameObject temp;

    bool spawnPlayer = false;

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

    void LateUpdate()
    {
        if (spawnPlayer)
        {
            ActualRespawn();
            spawnPlayer = false;
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if(evt.eventName == EventName.RespawnPlayer)
        {
            spawnPlayer = true;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

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

    public void SetFuelCount(int count)
    {
        fuelCount = count;
    }

    public Vector3 GetRespawnPosition()
    {
        return position;
    }

    public Vector3 GetRespawnRotation()
    {
        return rotation;
    }

    void ActualRespawn()
    {
        GameObject go = Instantiate(playerPrefab, position + (rotation * spawnDistance), Quaternion.identity) as GameObject;
        go.transform.LookAt(position + (rotation * (spawnDistance+1)), Vector3.up);
        go.GetComponentInChildren<OxygenController>().SetOxygen(++fuelCount);
        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, go.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
    }
}