using UnityEngine;
using System.Collections;

public class MalfunctioningDoors : MonoBehaviour {

    public bool doorIsMalfunctioning = true;

    [Header("The Random range between Close/Open doors:")]
    public float minRange = 2f;
    public float maxRange = 4f;

    private bool malfunctioning = false;
    private bool closed = true;

    [HideInInspector]
    public HazardState state;
    private Animator animator;

    [HideInInspector]
    public int doorsTouchingPlayer = 0;
    private bool crushingPlayer = false;

    public void CloseOpenDoor()
    {
        if (!state.isOn || closed)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            animator.SetTrigger("Close");
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
    void Start () {
        state = gameObject.GetComponent<HazardState>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (doorsTouchingPlayer >= 2 && !crushingPlayer &&
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("DoorClose"))
        {
            var evt = new ObserverEvent(EventName.Crushed);
            Subject.instance.Notify(gameObject, evt);
            crushingPlayer = true;

            Debug.Log("Player has been crushed!");
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
