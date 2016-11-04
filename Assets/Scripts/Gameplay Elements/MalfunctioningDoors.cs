using UnityEngine;
using System.Collections;

public class MalfunctioningDoors : MonoBehaviour {

    public bool doorIsMalfunctioning = true;

    private bool malfunctioning = false;
    private bool closed = true;

    [HideInInspector]
    public HazardState state;
    private Animator animator;

    public void CloseOpenDoor()
    {
        //yield return new WaitForSeconds(Random.Range[2, 4]);
        if (!state.isOn || closed)
        {
            Debug.Log("opening doors");
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("closing doors");
            animator.SetTrigger("Close");
        }
    }

    public IEnumerator Malfunctioning()
    {
        while (malfunctioning)
        {
            Debug.Log("doors are malfunctioning");
            CloseOpenDoor();
            closed = !closed;
            yield return new WaitForSeconds(Random.Range(2, 4));
        }
    }

    // Use this for initialization
    void Start () {
        state = gameObject.GetComponent<HazardState>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
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
