using UnityEngine;
using System.Collections;

public class TutorialBehaviour : MonoBehaviour {

    public GameObject playerCamera;
    public GameObject staticCamera;
    public InputController input;
    public Transform key;
    private Animator animator;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Tutorial Room"))
        {
            coll.gameObject.SetActive(false);
            SetStaticCamera(true);
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 4.5f;
        }
        else if (coll.CompareTag("Tutorial Trigger"))
        {
            coll.gameObject.SetActive(false);
            key.gameObject.SetActive(true);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetTrigger("Missing Keys");
            var statusEvent = new ObserverEvent(EventName.DisableInput);
            Subject.instance.Notify(gameObject, statusEvent);
            Invoke("RotateCamera", 2.5f);
        }
        else if (coll.CompareTag("Tutorial Goal"))
        {
            // TODO: make door open and key move out
            var evt = new ObserverEvent(EventName.PlayerWon);
            Subject.instance.Notify(gameObject, evt);
        }
    }

    void SetStaticCamera(bool isActive)
    {
        playerCamera.SetActive(!isActive);
        staticCamera.SetActive(isActive);
    }

    void RotateCamera()
    {
        SetStaticCamera(false);
        input.SetViewDirection(key.position);
        var statusEvent = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, statusEvent);
    }
}
