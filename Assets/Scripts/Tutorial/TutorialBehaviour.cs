using UnityEngine;
using System.Collections;

public class TutorialBehaviour : MonoBehaviour {

    public InputController input;
    public Transform goal;
    public Transform key;
    private Animator animator;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Tutorial Trigger"))
        {
            coll.gameObject.SetActive(false);
            goal.gameObject.SetActive(true);
            key.gameObject.SetActive(true);
            animator.SetTrigger("Missing Keys");
            var statusEvent = new ObserverEvent(EventName.DisableInput);
            Subject.instance.Notify(gameObject, statusEvent);
            Invoke("RotateCamera", 2.5f);
        }
        else if (coll.CompareTag("Tutorial Goal"))
        {
            var evt = new ObserverEvent(EventName.PlayerWon);
            Subject.instance.Notify(gameObject, evt);
        }
    }

    void RotateCamera()
    {
        input.SetViewDirection(goal.position);
        var statusEvent = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, statusEvent);
    }
}
