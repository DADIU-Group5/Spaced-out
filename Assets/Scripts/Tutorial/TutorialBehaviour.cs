using UnityEngine;
using System.Collections;

public class TutorialBehaviour : MonoBehaviour {

    public GameObject playerCamera;
    public GameObject staticCamera;
    public GameObject doorCamera;
    public GameObject keysCamera;
    public InputController input;
    public Transform key;

    public GameObject tutorialTrigger; 

    private Animator animator;
    private PlayerController playerController;

	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
        playerController = GetComponent<PlayerController>();
	}

    void Update()
    {
        CheckTutorialAim();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Tutorial Room"))
        {
            coll.gameObject.SetActive(false);
            SetStaticCamera();
            GetComponent<Rigidbody>().velocity = new Vector3(7.5f, 0, 0);
        }
        else if (coll.CompareTag("Tutorial Door"))
        {
            coll.gameObject.SetActive(false);
            SetDoorCamera();
            Invoke("ToggleKeyTrigger", 2.5f);
        }
        else if (coll.CompareTag("Tutorial Trigger"))
        {
            coll.gameObject.SetActive(false);
            key.gameObject.SetActive(true);
            Invoke("StartMovingCamera", 2.5f);
            GetComponent<PlayerController>().ReadyForLaunch();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetTrigger("Missing Keys");
            var statusEvent = new ObserverEvent(EventName.DisableInput);
            Subject.instance.Notify(gameObject, statusEvent);
        }
    }

    void SetStaticCamera()
    {
        playerCamera.SetActive(false);
        staticCamera.SetActive(true);
    }

    void StartMovingCamera()
    {
        doorCamera.SetActive(false);
        staticCamera.SetActive(false);
        keysCamera.SetActive(true);
        keysCamera.GetComponent<TutorialCamera>().Animate();
    }

    void SetDoorCamera()
    {
        staticCamera.SetActive(false);
        doorCamera.SetActive(true);
    }

    private void ToggleKeyTrigger()
    {
        tutorialTrigger.SetActive(true);
    }

    public void EnableControl()
    {
        keysCamera.SetActive(false);
        playerCamera.SetActive(true);
        input.SetViewDirection(key.position);
        var statusEvent = new ObserverEvent(EventName.EnableInput);
        Subject.instance.Notify(gameObject, statusEvent);
    }

    // get point where the player is aiming
    private void CheckTutorialAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;

        // Create layermask that ignores all Golfball and Ragdoll layers
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int finalmask = ~(layermask1 | layermask2);

        if (Physics.Raycast(ray, out hit, float.MaxValue, finalmask))
        {
            if (hit.transform.CompareTag("Tutorial Aim"))
            {
                Debug.Log("Hit correctly");
            }
        }
    }

    // Returns the pixel center of the camera.
    private Vector2 ScreenCenter()
    {
        return new Vector2(Camera.main.pixelWidth / 2f, Camera.main.pixelHeight / 2f);
    }
}
