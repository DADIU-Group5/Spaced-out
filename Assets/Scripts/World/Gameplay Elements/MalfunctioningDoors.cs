using UnityEngine;
using System.Collections;

public class MalfunctioningDoors : MonoBehaviour {

    public bool doorIsMalfunctioning = true;
    bool staticDoor = true;

    [Header("The Random range between Close/Open doors:")]
    public float minRange = 1f;
    public float maxRange = 4f;

    public GameObject[] particles;

    private bool malfunctioning = false;
    private bool closed = true;

    bool started = false;

    [HideInInspector]
    private Animator animator;

    public void CloseOpenDoor()
    {
        //var evt = new ObserverEvent(EventName.Door);
        //evt.payload.Add(PayloadConstants.DOOR_OPEN, gameObject);
        //doorOpenTrigger.Open();
        if (closed)
        {
            animator.SetTrigger("Open");
            AkSoundEngine.PostEvent(SoundEventConstants.DOOR_OPEN, gameObject);
            //doorOpenTrigger.Open();
            //evt.payload.Add(PayloadConstants.DOOR_OPEN, true);
        }
        else
        {
            animator.SetTrigger("Close");
            AkSoundEngine.PostEvent(SoundEventConstants.DOOR_SHUT, gameObject);
            //doorCloseTrigger.Close();
            //evt.payload.Add(PayloadConstants.DOOR_OPEN, false);
        }
        
        //Subject.instance.Notify(gameObject, evt);
    }

    public void LockDoor()
    {
        doorIsMalfunctioning = false;
        animator.SetTrigger("Locked");
        closed = false;
        DisableParticles();
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
        AkSoundEngine.PostEvent(SoundEventConstants.DOOR_OPEN, gameObject);
        closed = false;
    }

    public void StartDoor()
    {
        started = true;
        if (staticDoor)
        {
            animator.SetTrigger("Open");
            AkSoundEngine.PostEvent(SoundEventConstants.DOOR_OPEN, gameObject);
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
        AkSoundEngine.PostEvent(SoundEventConstants.DOOR_OPEN, gameObject);
        closed = false;
        if (doorIsMalfunctioning)
        {
            EnableParticles();
        }
        else
        {
            DisableParticles();
        }
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
            EnableParticles();
            staticDoor = false;
        }
        else
        {
            DisableParticles();
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

    void EnableParticles()
    {
        foreach (GameObject item in particles)
        {
            item.SetActive(true);
        }
    }

    void DisableParticles()
    {
        foreach (GameObject item in particles)
        {
            item.SetActive(false);
        }
    }
}
