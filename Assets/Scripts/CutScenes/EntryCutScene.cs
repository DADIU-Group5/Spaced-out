using UnityEngine;
using System.Collections;

public class EntryCutScene : MonoBehaviour {

    public GameObject keyPrefab;
    public Transform keyAnimParent;
    public Transform playerAnimParent;
    public Camera cutsceneCam;

    private Animator cutsceneAnimator;
    private Transform player;
    private GameObject particles;

   void ToggleUI()
    {
        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);
    }

	public void StartCutScene(GameObject playerObj)
    {
        ToggleUI();
        
        // create a key model for the animation
        GameObject keyModel = Instantiate(keyPrefab, keyAnimParent.transform.position, Quaternion.identity, keyAnimParent.transform) as GameObject;
        keyModel.GetComponent<SphereCollider>().enabled = false;

        // add player to cutscene animations 
        player = playerObj.transform;
        player.parent = playerAnimParent;
        player.localPosition = Vector3.zero;
        player.localRotation = Quaternion.identity;
        player.GetComponent<PlayerController>().Aim(keyAnimParent.transform.position);

        // set animation and particle effect
        player.GetComponentInChildren<Animator>().SetBool("Force Fly", true);
        particles = player.GetComponent<PlayerController>().chargeParticle;
        player.GetComponent<PlayerController>().chargeParticle = null;
        particles.SetActive(true);

        cutsceneCam.gameObject.SetActive(true);

        // play the cutscene animation
        GetComponent<Animator>().SetTrigger("Start");
    }

    // event from animator once player starts to move in cutscene
    public void StartPlayerFly()
    {
        var evt = new ObserverEvent(EventName.PlayerLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, 0.5f);
        evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, player.transform.forward);
        evt.payload.Add(PayloadConstants.START_STOP, true);
        Subject.instance.Notify(player.gameObject, evt);
    }

    // event from animator once player stop moving in cutscene
    public void StopPlayerFly()
    {
        particles.SetActive(false);
        player.gameObject.GetComponent<PlayerController>().chargeParticle = particles;
        player.GetComponentInChildren<Animator>().SetBool("Force Fly", false);
    }

    // event from animator when animation is over
    public void Ended()
    {
        ToggleUI();
        Destroy(cutsceneCam.gameObject);
        keyAnimParent.gameObject.SetActive(false);
        player.parent = null;

        CheckpointManager.instance.SetNewCheckpoint(playerAnimParent.position - new Vector3(-2, 0, 0));
        CheckpointManager.instance.SetNewCheckpointRotation(playerAnimParent.forward);

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, player.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
    }

    public Transform GetPlayerSpawnPos()
    {
        return playerAnimParent;
    }
}
