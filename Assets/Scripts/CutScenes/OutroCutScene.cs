using UnityEngine;
using System.Collections;

public class OutroCutScene : MonoBehaviour {

    public Animator anim;
    public Camera cam;
    public Transform playerPos;

    public void StartOutro(GameObject player)
    {

        player.transform.position = playerPos.position;
        player.transform.rotation = playerPos.rotation;
        player.transform.parent = playerPos;
        cam.gameObject.SetActive(true);

        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);

        evt = new ObserverEvent(EventName.StartCutscene);
        Subject.instance.Notify(gameObject, evt);

        anim.SetTrigger("Open");
    }

    public void Ended()
    {
        var evt = new ObserverEvent(EventName.PlayerWon);
        Subject.instance.Notify(gameObject, evt);
    }
}
