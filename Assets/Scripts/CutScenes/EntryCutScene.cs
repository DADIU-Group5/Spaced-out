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

    //[Header("Cameras")]
    //public GameObject mainCameraPod;
    //public GameObject zoomCamera;

    //public float zoomSpeed = 2.0f;
    //public GameObject finalKey;
    //public GameObject moveDirection;
    //public GameObject playerStartPosition;
   
    //private Vector3 keyPosition;
    //private Vector3 orgPosition;

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

    void ZoomInOnKey()
    {
        //if (zoomCamera == null)
        //{
        //    return;
        //}
        //player = GameObject.FindGameObjectWithTag("Player");
        //key.SetActive(false);
        //zoomCamera.SetActive(true);
        //keyPosition = finalKey.transform.position;
        //orgPosition = zoomCamera.transform.position;
        //zoomingIn = true;
        //player.GetComponentInChildren<Animator>().SetTrigger("StartCinematicFly");
        //player.GetComponentInChildren<Animator>().SetBool("CinematicFly", true);
    }

    void Update()
    {
        //if (zoomingIn)
        //{
        //    float step = zoomSpeed * Time.deltaTime;
        //    zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

        //    //if we're in range, stop zooming.
        //    if (Vector3.Distance(zoomCamera.transform.position, keyPosition) < 2f)
        //    {
        //        zoomingIn = false;
        //        StartCoroutine(waitAfterZooming());
        //        player = GameObject.FindGameObjectWithTag("Player");
        //        keyPosition = moveDirection.transform.position;
        //        //movingPlayer = true;
        //        player.transform.position = playerStartPosition.transform.position;
        //        player.GetComponentInChildren<Animator>().SetBool("CinematicFly", false);
        //        //GameObject.Find("Behind Camera Pod").GetComponent<LineRenderer>().enabled = false;
        //    }
        //}

        //if (backToDave)
        //{
        //    float step = zoomSpeed*2 * Time.deltaTime;
        //    zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, orgPosition, step);

        //    //if we're in range, stop zooming.
        //    if (Vector3.Distance(zoomCamera.transform.position, mainCameraPod.transform.position) < 10f)//(Vector3.Distance(zoomCamera.transform.position, orgPosition) < 1f)
        //    {
        //        backToDave = false;
        //        zoomCamera.SetActive(false);
        //        mainCameraPod.SetActive(true);
        //        GameObject.Find("Behind Camera Pod").GetComponent<LineRenderer>().enabled = true;
        //        //ToggleUI();
        //    }
        //}

        //if (movingPlayer)
        //{
            
        //    float step = zoomSpeed * Time.deltaTime;
        //    player.transform.position = Vector3.MoveTowards(player.transform.position, keyPosition, step);
        //    if (Vector3.Distance(player.transform.position, keyPosition) < 5)
        //    {
        //        movingPlayer = false;
        //    }
        //}
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
