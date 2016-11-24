using UnityEngine;

public class OutroCutScene : MonoBehaviour, Observer {
    public GameObject keyPrefab;
    public Animator anim;
    public Camera cam;
    public Transform playerPos;
    public Transform keyPos;

    void Start()
    {
        Subject.instance.AddObserver(this);
        Instantiate(keyPrefab,keyPos.transform.position, Quaternion.identity, keyPos);
    }

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

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.PlayerWon)
        {
            StartOutro(entity);
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
