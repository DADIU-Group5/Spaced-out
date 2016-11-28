﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialStage2 : MonoBehaviour, Observer
{
    public GameObject player;
    public SimpelAnimation door;

    [Header("Cameras")]
    public GameObject playerCameraPod;
    public GameObject introCamera;

    // Use this for initialization
    void Start()
    {
        Subject.instance.AddObserver(this);

        Brain.instance.randomRemarks = false;

        // setup camera and disable inputs
        var evt = new ObserverEvent(EventName.DisableInput);
        Subject.instance.Notify(gameObject, evt);
        
        ToggleUI();

        playerCameraPod.SetActive(false);
        introCamera.gameObject.SetActive(true);

        StartCoroutine(BeginIntroCutscene());

        Invoke("NarrateOnDavesLuck", 12.0f);
    }

    private IEnumerator BeginIntroCutscene()
    {
        yield return new WaitForSeconds(1f);
        door.PlayAnimations(() => { });
        // launch player
        var controller = player.GetComponent<PlayerController>();
        controller.Aim(player.transform.position + Vector3.forward);
        controller.SetPower(0.45f);
        yield return new WaitForSeconds(0.2f); // wait to let animator finish transistion
        controller.Launch();
    }

    private void NarrateOnDavesLuck()
    {
        Brain.instance.Narrate("narrative8");
    }

    private void ToggleUI()
    {
        var statusEvent = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, statusEvent);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.RespawnPlayer)
        {
            var evt2 = new ObserverEvent(EventName.EnableInput);
            Subject.instance.Notify(gameObject, evt2);
            evt2 = new ObserverEvent(EventName.ToggleUI);
            Subject.instance.Notify(gameObject, evt2);
            playerCameraPod.SetActive(true);
            introCamera.gameObject.SetActive(false);
        }
        else if (evt.eventName == EventName.PlayerWon)
        {
            SceneManager.LoadScene("TutStage03");
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}