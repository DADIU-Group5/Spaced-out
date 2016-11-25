using UnityEngine;

public class OutroCutScene : MonoBehaviour, Observer {
    public GameObject keyPrefab;
    public Animator anim;
    public Camera cam;
    public Transform playerPos;
    public Transform keyPos;

    GameObject particles;

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

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        player.GetComponent<PlayerController>().OverrideReadyForLaunch();
        player.GetComponent<PlayerController>().Aim(keyPos.transform.position);

        cam.gameObject.SetActive(true);
        player.GetComponentInChildren<Animator>().ResetTrigger("Pick Up");
        player.GetComponentInChildren<Animator>().SetTrigger("FakeFly");


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

    public void StartFly()
    {
        var evt = new ObserverEvent(EventName.PlayerLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, 0.5f);
        evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, playerPos.transform.forward);
        evt.payload.Add(PayloadConstants.START_STOP, true);
        Subject.instance.Notify(playerPos.gameObject, evt);
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        if (evt.eventName == EventName.PlayerGotKey)
        {
            StartOutro(entity);
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
