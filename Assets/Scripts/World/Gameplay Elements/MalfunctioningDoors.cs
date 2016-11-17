using UnityEngine;
using System.Collections;

public class MalfunctioningDoors : MonoBehaviour {

    public bool doorIsMalfunctioning = true;
    bool staticDoor = true;

    [Header("The Random range between Close/Open doors:")]
    public float minRange = 1f;
    public float maxRange = 4f;

    private bool malfunctioning = false;
    private bool closed = true;

    bool started = false;

    [HideInInspector]
    private Animator animator;

    public void CloseOpenDoor()
    {
        if (closed)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
        }
    }

    public void LockDoor()
    {
        doorIsMalfunctioning = false;
        animator.SetTrigger("Locked");
        closed = false;
    }

    public void DoorLocked()
    {
        transform.parent.GetComponent<InRoomDoor>().DoorLocked();
    }

    public void UnlockDoor()
    {
        if (staticDoor)
        {
            return;
        }
        doorIsMalfunctioning = false;
        animator.SetTrigger("Open");
        closed = false;
    }

    public void StartDoor()
    {
        started = true;
        if (staticDoor)
        {
            animator.SetTrigger("Open");
            closed = false;
        }
    }

    public void Switch()
    {
        if (staticDoor)
        {
            return;
        }
        doorIsMalfunctioning = !doorIsMalfunctioning;
        animator.SetTrigger("Open");
        closed = false;
    }

    public IEnumerator Malfunctioning()
    {
        while (malfunctioning)
        {
            CloseOpenDoor();
            closed = !closed;
            yield return new WaitForSeconds(Random.Range(minRange, maxRange));
        }
    }

    // Use this for initialization
    void Awake () {
        animator = gameObject.GetComponent<Animator>();
        if(Random.Range(0,2) == 0)
        {
            staticDoor = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (staticDoor)
        {
            return;
        }
        if (!started)
        {
            return;
        }

        if (doorIsMalfunctioning && !malfunctioning)
        {
            
            malfunctioning = true;
            StartCoroutine(Malfunctioning());
        }
        else if (!doorIsMalfunctioning)
        {
            malfunctioning = false;
        }
    }
}
