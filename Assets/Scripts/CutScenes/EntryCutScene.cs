using UnityEngine;
using System.Collections;

public class EntryCutScene : MonoBehaviour {

    public GameObject keyPrefab;
    public Animator anim;
    public Camera cam;
    public Transform playerPos;
    public GameObject key;
    Transform playerObj;
    GameObject particles;

    void Awake()
    {
        GameObject keyModel = Instantiate(keyPrefab, key.transform.position, Quaternion.identity, key.transform) as GameObject;
        keyModel.GetComponent<SphereCollider>().enabled = false;
        //Camera.main.transform.parent.parent.GetComponent<InputController>().SetViewDirection(key.transform.position);
    }

	public void StartCutScene(GameObject player)
    {
        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);

        player.GetComponent<PlayerController>().Aim(key.transform.position);



        playerObj = player.transform;

        playerObj.position = playerPos.position;
        playerObj.rotation = playerPos.rotation;
        playerObj.parent = playerPos;
        player.GetComponentInChildren<Animator>().SetTrigger("FakeFly");
        particles = player.GetComponent<PlayerController>().chargeParticle;
        player.GetComponent<PlayerController>().chargeParticle = null;
        particles.SetActive(true);
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
        CheckpointManager.instance.SetNewCheckpointRotation(playerPos.forward);

        cam.gameObject.SetActive(false);

        particles.SetActive(false);
        playerObj.gameObject.GetComponent<PlayerController>().chargeParticle = particles;
        playerObj.gameObject.GetComponentInChildren<Animator>().SetTrigger("Ready To Launch");

        evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, playerObj.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);

    }

    public Transform GetPlayerSpawnPos()
    {
        return playerPos;
    }
}
