﻿using UnityEngine;
using System.Collections;

public class TutorialStage3 : MonoBehaviour {

    public EntryCutScene ECS;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    [Header("Cameras")]
    public GameObject playerCameraPod;
    public SimpelAnimation oxygenCamera;
    public SimpelAnimation hazardCamera;
    [Header("Triggers")]
    public TutorialTrigger secondRoom;

	// Use this for initialization
	void Start () {
        GameObject go = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
        go.transform.LookAt(transform.position, Vector3.up);
        go.GetComponentInChildren<OxygenController>().SetOxygen(1);
        CheckpointManager.instance.SetFuelCount(go.GetComponentInChildren<OxygenController>().GetOxygen());

        var evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);
        ECS.StartCutScene(go);

        // play camera animation after small delay
        Invoke("PlayZoomOxygenAnimation", 1f);
        secondRoom.callback = PlayZoomHazardAnimation;
    }

    private void PlayZoomOxygenAnimation()
    {
        // disable input
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);

        playerCameraPod.SetActive(false);
        oxygenCamera.gameObject.SetActive(true);
        oxygenCamera.PlayAnimations(EnablePlayerControl);
    }

    public void PlayZoomHazardAnimation()
    {
        // disable input
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);

        playerCameraPod.SetActive(false);
        hazardCamera.gameObject.SetActive(true);
        hazardCamera.PlayAnimations(EnablePlayerControl);
    }

    // enable player controls again
    private void EnablePlayerControl()
    {
        var evt = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, evt);
        playerCameraPod.SetActive(true);
        oxygenCamera.gameObject.SetActive(false);
        hazardCamera.gameObject.SetActive(false);
    }
}
