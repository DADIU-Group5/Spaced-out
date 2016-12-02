using UnityEngine;
using System.Collections;

public class FloatBy : MonoBehaviour {

    [Header("Cameras")]
    public GameObject doorCameraPod;
    public GameObject floatCamera;
    public GameObject WinMenu;
    public GameObject[] patrolPoints;
    public float speed = 3.0f;
    public float step;
    public GameObject KeyPrefab;
    private GameObject key;
    private bool patrolling = false;
    private int currentPatrolPoint = 0;
    private int currentKeyPatrolPoint = 0;
    private Vector3 currentPatrolPosition;
    private Vector3 currentPatrolKeyPosition;
    private GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           // ToggleUI();
            doorCameraPod.SetActive(false);
            floatCamera.gameObject.SetActive(true);

            for (int i = 0; i < WinMenu.transform.childCount; i++)
            {
                WinMenu.transform.GetChild(i).gameObject.SetActive(true);
            }

            player = other.gameObject;
            currentPatrolPosition = patrolPoints[currentPatrolPoint].transform.position;

            //Let the chaos ensue!
            patrolling = true;
            player.GetComponentInChildren<Animator>().SetTrigger("StartCinematicFly");
            player.GetComponentInChildren<Animator>().SetBool("CinematicFly", true);

            key = KeyPrefab;
            currentPatrolKeyPosition = patrolPoints[currentKeyPatrolPoint].transform.position;
        }
    }

    Quaternion q;

    void Update()
    {
        if (patrolling)
        {
            //move the player towards the current patrol point:
            step = speed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentPatrolPosition, step);

            player.transform.LookAt(currentPatrolPosition);

            //if at point, choose another:
            if (Vector3.Distance( player.transform.position, currentPatrolPosition) < 5f)
            {
                currentPatrolPoint++;
                currentPatrolPoint = currentPatrolPoint+1 <= patrolPoints.Length ? currentPatrolPoint++ : currentPatrolPoint = 0;
                currentPatrolPosition = patrolPoints[currentPatrolPoint].transform.position;        
            }


            key.transform.position = Vector3.MoveTowards(key.transform.position, currentPatrolKeyPosition+ new Vector3(5,0,7), step);

            //if at point, choose another:
            if (Vector3.Distance(key.transform.position, currentPatrolKeyPosition + new Vector3(5, 0, 7)) < 1f)
            {
                currentKeyPatrolPoint++;
                currentKeyPatrolPoint = currentKeyPatrolPoint + 1 <= patrolPoints.Length ? currentKeyPatrolPoint++ : currentKeyPatrolPoint = 0;
                currentPatrolKeyPosition = patrolPoints[currentKeyPatrolPoint].transform.position;
            }
        }
    }
}
