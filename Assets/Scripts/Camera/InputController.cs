using UnityEngine;

/// <summary>
/// This class is responsible for catching inputs from the player and call the according methods on the camera and player controller.
/// It should NOT be responsible for determining if and how the player can perform an action. 
/// </summary>
[RequireComponent(typeof(CameraController))]
public class InputController : MonoBehaviour, Observer
{
    public PlayerController player;
    public float playerRotateSpeed = 200f;
    public float cameraRotateSpeed = 4000f,
        launchBuffer = 100f;
    public Collider hitboxCollider;
    //public Transform playerPitchTransform;
    private CameraController cameraController;
    private bool invertCameraControls;
    private bool launchMode;
    private bool inputDisabled;
    private bool cameraInputDisabled;
    private Vector2 oldPoint;
    private Camera cam;
    private bool paused = false;

    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
        cam = Camera.main;
        invertCameraControls = SettingsManager.instance.settings.invertedCamera;
        Subject.instance.AddObserver(this);

        inputDisabled = false;
        cameraInputDisabled = false;
    }

    private void LateUpdate()
    {
        // If input is disabled, stop.
        if (inputDisabled)
        {
            return;
        }

        // Sets launchmode if two fingers are registered.
        if (Input.GetMouseButtonDown(1))
        {
            //launchMode = DetectPlayerTap();
            launchMode = true;
        }

        // Check if we are NOT in launchmode and camera input NOT disabled
        if (!launchMode && !cameraInputDisabled)
        {
            HandleCameraMode();
        }
        else if (launchMode)
        {
            HandleLaunchMode();
        }
    }

    // sets the view direction of the camera and player
    public void SetViewDirection(Vector3 point)
    {
        Vector3 direction = (point - player.transform.position).normalized;
        Vector3 rotation = Quaternion.LookRotation(direction).eulerAngles;

        player.Aim(point);
        cameraController.transform.rotation = Quaternion.Euler(rotation.x, 0, 0);
        cameraController.pitch.transform.rotation = Quaternion.Euler(0, rotation.y, 0);
    }

    // Interprest input as launch mode.
    private void HandleLaunchMode()
    {
        // Save starting position of tap
        if (Input.GetMouseButtonDown(0))
        {
            oldPoint = Input.mousePosition;
        }

        // Launch 
        if (Input.GetMouseButtonUp(0))
        {
            player.Launch();
            launchMode = false;
            return;
        }

        // Rotate player pitch so it faces camera direction and update velocity meter according to where finger is on the screen
        if (Input.GetMouseButton(0))
        {
            //playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;
            player.SetPower(GetLaunchPower());
        }
    }

    // Interprets input as camera mode.
    protected virtual void HandleCameraMode()
    {
        if (!paused)
        {
            // Save starting position of tap
            if (Input.GetMouseButtonDown(0))
            {
                oldPoint = Input.mousePosition;
            }

            // Look around and change position of camera
            if (Input.GetMouseButton(0))
            {
                Vector2 pos = Input.mousePosition;
                Vector2 offset = pos - oldPoint;

                if (invertCameraControls)
                {
                    offset = -offset;
                }

                DirectedRotation(offset);
                oldPoint = pos;
                player.Aim(GetAimPoint());
            }
        }
    }

    // Returns true if player was tapped, otherwise false.
    private bool DetectPlayerTap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return hitboxCollider.Raycast(ray, out hit, 1000f);
    }

    // Calculate power from old mouse position and current mouse position.
    private float GetLaunchPower()
    {
        float difference = oldPoint.y - Input.mousePosition.y;
        float maxDifference = oldPoint.y - launchBuffer;

        return (difference / maxDifference).Clamp(0f, 1f);
    }

    // get point where the player is aiming
    public Vector3 GetAimPoint()
    {
        Ray ray = cam.ScreenPointToRay(ScreenCenter()); 
        RaycastHit hit;

        // Create layermask that ignores all Golfball and Ragdoll layers
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int layermask3 = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int finalmask = ~(layermask1 | layermask2 | layermask3);

        if (Physics.Raycast(ray, out hit, float.MaxValue, finalmask))
        {
            return hit.point;
        }
        else
        {
            return player.transform.position + player.transform.forward;
        }
    }
    
    // Calculate the direction from the character position and the crosshair.
    public Vector3 GetLaunchDirection()
    {
        return (GetAimPoint() - player.transform.position).normalized;
    }

    // Returns the pixel center of the camera.
    private Vector2 ScreenCenter()
    {
        return new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f);
    }

    // Rotates the assigned behindCamera in the direction of the offset.
    private void DirectedRotation(Vector2 offset)
    {
        float xScale = cameraController.pitch.transform.up.y;
        xScale = Mathf.Sign(xScale);
        transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude), Space.World);
        cameraController.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        
        // Clamp rotation
        // This is fucking weird and stupid, but it works. Ask Malte to explain if you need to know about it..
        if (Mathf.Abs(cameraController.pitch.transform.localEulerAngles.z - 180f) < 0.1)
        {
            if (cameraController.pitch.transform.localEulerAngles.x < 180)
            {
                cameraController.pitch.transform.localEulerAngles = new Vector3(89.99f, 0f, 0f);
            }
            else
            {
                cameraController.pitch.transform.localEulerAngles = new Vector3(270.01f, 0f, 0f);
            }
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerSpawned:
                inputDisabled = false;
                cameraInputDisabled = false;
                GameObject go = evt.payload[PayloadConstants.PLAYER] as GameObject;
                player = go.GetComponent<PlayerController>();

                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.Rotate(new Vector3(0, player.transform.eulerAngles.y - transform.eulerAngles.y, 0));
                cameraController.pitch.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                gameObject.SetActive(true);
                break;
            case EventName.DisableInput:
            case EventName.StartCutscene:
            case EventName.PlayerWon:
            case EventName.PlayerDead:
                inputDisabled = true;
                cameraInputDisabled = true;
                break;
            case EventName.ToggleCameraControls:
                invertCameraControls = !invertCameraControls;
                break;
            case EventName.EnableInput:
                inputDisabled = false;
                cameraInputDisabled = false;
                break;
            case EventName.DisableCameraInput:
                cameraInputDisabled = true;
                break;
            case EventName.EnableCameraInput:
                cameraInputDisabled = false;
                break;
            case EventName.Pause:
                paused = !paused;
                inputDisabled = paused; //if paused, input is disabled = true.
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
