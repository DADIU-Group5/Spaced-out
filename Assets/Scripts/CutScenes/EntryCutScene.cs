using UnityEngine;
using System.Collections;

public class EntryCutScene : MonoBehaviour {

    public Animator anim;
    public Camera cam;
    public Transform playerPos;
    public GameObject key;
    Transform playerObj;

	public void StartCutScene(GameObject player)
    {
        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);

        //TODO: Remove UI.
        playerObj = player.transform;

        playerObj.position = playerPos.position;
        playerObj.rotation = playerPos.rotation;
        playerObj.parent = playerPos;
        cam.gameObject.SetActive(true);
        anim.SetTrigger("Start");
    }

    public void Ended()
    {
        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);
        Destroy(cam.gameObject);
        key.SetActive(false);
        playerObj.parent = null;

        CheckpointManager.instance.SetNewCheckpoint(playerPos.position);
        CheckpointManager.instance.SetNewCheckpointRotation(playerPos.right);

        cam.gameObject.SetActive(false);


        evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, playerObj.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
        //TODO: enable player input.
        //TODO: Show UI.
    }

    public Transform GetPlayerSpawnPos()
    {
        return playerPos;
    }
}
