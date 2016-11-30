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

    [Header("Cameras")]
    public GameObject mainCameraPod;
    public GameObject zoomCamera;

    public float zoomSpeed = 2.0f;
    public GameObject finalKey;
    public GameObject moveDirection;
   
    private Vector3 keyPosition;
    private Vector3 orgPosition;
    private bool zoomingIn = false;
    private bool backToDave = false;
    private bool movingPlayer = false;
    private GameObject player;

    void Awake()
    {
        GameObject keyModel = Instantiate(keyPrefab, key.transform.position, Quaternion.identity, key.transform) as GameObject;
        keyModel.GetComponent<SphereCollider>().enabled = false;
        //Camera.main.transform.parent.parent.GetComponent<InputController>().SetViewDirection(key.transform.position);
    }

   void ToggleUI()
    {
        var evt = new ObserverEvent(EventName.ToggleUI);
        Subject.instance.Notify(gameObject, evt);
    }

	public void StartCutScene(GameObject player)
    {
        ToggleUI();
        
        player.GetComponent<PlayerController>().Aim(key.transform.position);
        
        playerObj = player.transform;

        playerObj.position = playerPos.position;
        playerObj.rotation = playerPos.rotation;
        playerObj.parent = playerPos;
        //player.GetComponentInChildren<Animator>().SetBool("Force Fly", true);
        //player.GetComponentInChildren<Animator>().SetTrigger("Pick Up");
        particles = player.GetComponent<PlayerController>().chargeParticle;
        player.GetComponent<PlayerController>().chargeParticle = null;

        particles.SetActive(true);
        cam.gameObject.SetActive(true);
        anim.SetTrigger("Start");
    }

    void ZoomInOnKey()
    {
        if (zoomCamera == null)
        {
            return;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        key.SetActive(false);
        zoomCamera.SetActive(true);
        keyPosition = finalKey.transform.position;
        orgPosition = zoomCamera.transform.position;
        zoomingIn = true;
        player.GetComponentInChildren<Animator>().SetTrigger("StartCinematicFly");
        player.GetComponentInChildren<Animator>().SetBool("CinematicFly", true);
    }

    void Update()
    {
        if (zoomingIn)
        {
            float step = zoomSpeed * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, keyPosition) < 2f)
            {
                zoomingIn = false;
                StartCoroutine(waitAfterZooming());
                player = GameObject.FindGameObjectWithTag("Player");
                keyPosition = moveDirection.transform.position;
                movingPlayer = true;
            }
        }

        if (backToDave)
        {
            float step = zoomSpeed*2 * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, orgPosition, step);

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, orgPosition) < 1f)
            {
                backToDave = false;
            }
        }

        if (movingPlayer)
        {
            float step = zoomSpeed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, keyPosition, step);
            if (Vector3.Distance(player.transform.position, keyPosition) < 5)
            {
                movingPlayer = false;
            }
        }
    }

    IEnumerator waitAfterZooming()
    {
        yield return new WaitForSeconds(0.5f);
        backToDave = true;
    }

    public void Ended()
    {

        ToggleUI();
        Destroy(cam.gameObject);
        key.SetActive(false);
        playerObj.parent = null;

        CheckpointManager.instance.SetNewCheckpoint(playerPos.position);
        CheckpointManager.instance.SetNewCheckpointRotation(playerPos.forward);

        //cam.gameObject.SetActive(false);

        var evt = new ObserverEvent(EventName.PlayerSpawned);
        evt.payload.Add(PayloadConstants.PLAYER, playerObj.GetComponentInChildren<PlayerController>().gameObject);
        Subject.instance.Notify(gameObject, evt);
    }

    public void StopPlayerFly()
    {
        particles.SetActive(false);
        playerObj.gameObject.GetComponent<PlayerController>().chargeParticle = particles;
        playerObj.GetComponentInChildren<Animator>().SetBool("CinematicFly", false);
        //playerObj.gameObject.GetComponentInChildren<Animator>().SetBool("Force Fly", false);
        ZoomInOnKey();
    }

    public void StartPlayerFly()
    {
        var evt = new ObserverEvent(EventName.PlayerLaunch);
        evt.payload.Add(PayloadConstants.LAUNCH_FORCE, 0.5f);
        evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, playerObj.transform.forward);
        evt.payload.Add(PayloadConstants.START_STOP, true);
        Subject.instance.Notify(playerObj.gameObject, evt);
    }

    public Transform GetPlayerSpawnPos()
    {
        return playerPos;
    }
}
