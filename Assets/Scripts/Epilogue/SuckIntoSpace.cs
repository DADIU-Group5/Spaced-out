using UnityEngine;
using System.Collections;

public class SuckIntoSpace : MonoBehaviour
{

    public GameObject suckTowardsPoint;
    private Vector3 suckInDirection;
    private GameObject player;
    private bool movingPlayer = false;
    public float speed = 2.0f;
    [Header("Cameras")]
    public GameObject playerCameraPod;
    public GameObject introCamera;

    private bool pastLastDoor = false;

    // Use this for initialization
    void Start()
    {
        suckInDirection = suckTowardsPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingPlayer)
        {
            float step = speed * Time.deltaTime;
           player.transform.position = Vector3.MoveTowards(player.transform.position, suckInDirection, step);
            //player.transform.Translate(suckInDirection * speed * Time.deltaTime);

            //if we're in range, stop the movement.
            if (Vector3.Distance(player.transform.position, suckInDirection) < 5f)
            {
                movingPlayer = false;
            }
        }


    }

    //suck the player into space
    IEnumerator suckPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            //If player hits checkpoint, do stuff.
            player = other.gameObject;
            movingPlayer = true;
            //StartCoroutine(suckPlayer());
        }
    }
}
