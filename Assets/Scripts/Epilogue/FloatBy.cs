using UnityEngine;
using System.Collections;

public class FloatBy : MonoBehaviour {

    [Header("Cameras")]
    public GameObject doorCameraPod;
    public GameObject floatCamera;
    public GameObject WinMenu;
    public GameObject[] patrolPoints;
    public float speed = 5.0f;
    public float step;
    private bool patrolling = false;
    private int currentPatrolPoint = 0;
    private Vector3 currentPatrolPosition;
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
        }
    }

    void Update()
    {
        if (patrolling)
        {
            //move the player towards the current patrol point:
            step = speed * Time.deltaTime;
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentPatrolPosition, step);

            //if at point, choose another:
            if (Vector3.Distance( player.transform.position, currentPatrolPosition) < 5f)
            {
                currentPatrolPoint++;
                currentPatrolPoint = currentPatrolPoint+1 <= patrolPoints.Length ? currentPatrolPoint++ : currentPatrolPoint = 0;
                currentPatrolPosition = patrolPoints[currentPatrolPoint].transform.position;
            }
        }
    }
}
