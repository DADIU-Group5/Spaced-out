using UnityEngine;
using System.Collections;

public class EntryCutScene : MonoBehaviour {

    public Animator anim;
    public Camera cam;
    public Transform playerPos;
    Transform playerObj;

	public void StartCutScne(GameObject player)
    {
        //TODO: Remove UI.
        playerObj = player.transform;

        playerObj.position = playerPos.position;
        playerObj.rotation = playerPos.rotation;
        playerObj.parent = playerPos;
        cam.gameObject.SetActive(true);
        var evt = new ObserverEvent(EventName.StartOutro);
        Subject.instance.Notify(gameObject, evt);
        anim.SetTrigger("Start");
    }

    public void Ended()
    {
        //TODO: enable player input.
        //TODO: Show UI.
    }
}
