using UnityEngine;
using System.Collections;

public class LightTestThings : MonoBehaviour {

    public GameObject playerPrefab;
    public EntryCutScene ECS;

	// Use this for initialization
	void Start () {
        CheckpointManager.instance.SetSpawnDistance(1 + 1);
        GameObject go = Instantiate(playerPrefab, ECS.GetPlayerSpawnPos().position, Quaternion.identity) as GameObject;
        go.transform.LookAt(transform.position, Vector3.up);

        var evt = new ObserverEvent(EventName.StartCutscene);
        evt.payload.Add(PayloadConstants.START_LEVEL, true);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(go);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
