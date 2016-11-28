using UnityEngine;
using System.Collections;
using System;

public class TutorialStage3 : MonoBehaviour, Observer {

    public EntryCutScene ECS;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    [Header("Cameras")]
    public GameObject playerCameraPod;
    public SimpelAnimation oxygenCamera;
    public SimpelAnimation hazardCamera;
    [Header("Triggers")]
    public TutorialTrigger secondRoom;

    public Brain galAI;

    private OxygenController oxygenController;

	// Use this for initialization
	void Start () {
        GameObject go = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
        go.transform.LookAt(transform.position, Vector3.up);

        oxygenController = GameObject.Find("Player(Clone)").GetComponent<OxygenController>();

        //go.GetComponentInChildren<OxygenController>().SetOxygen(1); // doesn't seem to be working
        //oxygenController = go.GetComponentInChildren<OxygenController>();

        galAI.randomRemarks = false;

        //ToggleUI();

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

        Invoke("NarrateJetPackFailure", 3.5f);
        StartCoroutine(ReduceOxygenCoroutine());
    }

    private void NarrateJetPackFailure()
    {
        galAI.Narrate("narrative9");
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

    IEnumerator ReduceOxygenCoroutine()
    {
        for (int i = 0; i < 9; i++)
        {
            oxygenController.UseOxygen();
            yield return new WaitForSeconds(0.7f);
        }

        CheckpointManager.instance.SetFuelCount(oxygenController.GetOxygen());
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.PlayerSpawned)
        {
            oxygenController.SetOxygen(2);
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
