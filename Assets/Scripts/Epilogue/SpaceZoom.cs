using UnityEngine;
using System.Collections;

public class SpaceZoom : MonoBehaviour {

    [Tooltip("Zoomspeed - it's doubled on the way 'back'.")]
    public float zoomSpeed = 5.0f;
    public float speed = 2.0f;
    public Vector3 keyPosition;  
    public GameObject suckTowardsPoint;
    public GameObject sucktionParticles;
    public GameObject Logo;
    public GameObject epilogueKey;
    
    [Header("Camera:")]
    public GameObject zoomCamera;
    public GameObject sucktionCamera;
    [Header("Key and camera startposition:")]
    public GameObject key;
    public GameObject startRotationPos;
    public GameObject pickUpKeyStartPos;

    private GameObject player;
    private bool movingPlayer = false;
    private bool zoomedInAlready = false;
    private Vector3 suckInDirection;
    private Quaternion orgRotation;
    private bool zoomingIn = false;
    private bool zoomingOut = false;
    private Vector3 orgPosition;

    void Start()
    {   //find the direction to suck the player out of the door.
        suckInDirection = suckTowardsPoint.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!zoomedInAlready)
            {   
                zoomCamera.SetActive(true);

                orgRotation = zoomCamera.transform.rotation;
                orgPosition = zoomCamera.transform.position;

                zoomCamera.transform.position = pickUpKeyStartPos.transform.position;
                zoomCamera.transform.rotation = pickUpKeyStartPos.transform.rotation;

                player = other.gameObject; //set the player, too.
            }
        }
    }

    public void StartZooming()
    {
        //set start-zooming position
        zoomCamera.transform.position = startRotationPos.transform.position;
        zoomCamera.transform.rotation = startRotationPos.transform.rotation;//orgRotation;
        //find the zoom target
        keyPosition = key.transform.position;
        //start zooming in
        zoomingIn = true;
        //avoid zooming again if player accidentally enters collider again
        zoomedInAlready = true;
    }

    void Update()
    {
        if (zoomingIn)
        {
            float step = zoomSpeed * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

            //rotate player camera to look at the logo.
            if (zoomCamera.transform.rotation.y < orgRotation.y + 10)
                zoomCamera.transform.RotateAround(zoomCamera.transform.position, Vector3.down + Vector3.forward*2f, 20 * Time.deltaTime);
              

            //if we're in range, stop zooming.
            if (Vector3.Distance(zoomCamera.transform.position, keyPosition) < 2f)
            {
                zoomingIn = false;
                StartCoroutine(waitAfterZooming());
            }
        }

        //player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, suckTowardsPoint.transform.rotation, 1f);
        // player.transform.position = Vector3.RotateTowards(other.transform.position, keyPosition, 1f, 1f);
    }

    //suck the player into space
    IEnumerator suckPlayer()
    {
        epilogueKey.SetActive(false);
        var controller = player.GetComponent<PlayerController>();
        controller.Aim(suckInDirection);
        controller.SetPower(1.45f);
        yield return new WaitForSeconds(0.2f); // wait to let animator finish transistion
        controller.Launch();
    }

    IEnumerator waitAfterZooming()
    {
        yield return new WaitForSeconds(0.5f);

        //save camera positions
        zoomCamera.transform.position = sucktionCamera.transform.position;
        zoomCamera.transform.rotation = orgRotation;

        //turn off the targeting system
        GameObject.Find("Behind Camera Pod").GetComponent<LineRenderer>().enabled = false;

        sucktionParticles.SetActive(true);

        player.transform.position = epilogueKey.transform.position;

        StartCoroutine(suckPlayer());

        Logo.SetActive(false);

        zoomCamera.transform.position = pickUpKeyStartPos.transform.position;
        zoomCamera.transform.rotation = pickUpKeyStartPos.transform.rotation;
    }
	
}
