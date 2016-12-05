using UnityEngine;
using System.Collections;

public class EpilogueSpawner : MonoBehaviour {

    public EntryCutScene ECS;
    public GameObject player;

	// Use this for initialization
	void Start () {
        //GameObject go = Instantiate(player,ECS.GetPlayerSpawnPos().position,ECS.GetPlayerSpawnPos().rotation) as GameObject;
        //go.transform.LookAt(transform.position + new Vector3(0, 2, 0), Vector3.up);

        //var evt = new ObserverEvent(EventName.PlayerSpawned);
        //evt.payload.Add(PayloadConstants.PLAYER, go.GetComponentInChildren<PlayerController>().gameObject);
        //Subject.instance.Notify(gameObject, evt);

        var evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(player);

    }
}
