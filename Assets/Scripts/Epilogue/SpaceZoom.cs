using UnityEngine;
using System.Collections;

public class SpaceZoom : MonoBehaviour {

    [Tooltip("Zoomspeed - it's doubled on the way 'back'.")]
    public float zoomSpeed = 5.0f;
    public float speed = 5.0f;
    public Vector3 keyPosition;  
    public GameObject suckTowardsPoint;
    public GameObject sucktionParticles;
    
    [Header("Camera:")]
    public GameObject zoomCamera;
    public GameObject sucktionCamera;
    [Header("Key and camera startposition:")]
    public GameObject key;
    public GameObject startPos;

    private GameObject player;
    private bool movingPlayer = false;
    private bool zoomedInAlready = false;
    private Vector3 suckInDirection;
    private Quaternion orgRotation;
    private bool zoomingIn = false;
    private bool zoomingOut = false;
    private Vector3 orgPosition;

    void Start()
    {
        suckInDirection = suckTowardsPoint.transform.position;
    }

    void Update()
    {
        if (zoomingIn)
        {
            float step = zoomSpeed * Time.deltaTime;
            zoomCamera.transform.position = Vector3.MoveTowards(zoomCamera.transform.position, keyPosition, step);

            if (zoomCamera.transform.rotation.y < orgRotation.y + 10)
                zoomCamera.transform.RotateAround(zoomCamera.transform.position, Vector3.forward + Vector3.right, 20 * Time.deltaTime);

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
        var controller = player.GetComponent<PlayerController>();
        controller.Aim(suckInDirection);
        controller.SetPower(1.45f);
        yield return new WaitForSeconds(0.2f); // wait to let animator finish transistion
        controller.Launch();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!zoomedInAlready)
            {
                zoomCamera.SetActive(true);
                zoomCamera.transform.position = startPos.transform.position;
                orgPosition = zoomCamera.transform.position;
                orgRotation = zoomCamera.transform.rotation;
                keyPosition = key.transform.position;
                zoomingIn = true;
                zoomedInAlready = true;
                player = other.gameObject;
                
            }
        }
    }

    IEnumerator waitAfterZooming()
    {
        yield return new WaitForSeconds(0.5f);
        zoomingOut = true;
        zoomCamera.transform.position = sucktionCamera.transform.position;
        zoomCamera.transform.rotation = orgRotation;
        GameObject.Find("Behind Camera Pod").GetComponent<LineRenderer>().enabled = false;

        //movingPlayer = true;
        sucktionParticles.SetActive(true);
        StartCoroutine(suckPlayer());

        //sucktionCamera.SetActive(true);
    }
	
}
