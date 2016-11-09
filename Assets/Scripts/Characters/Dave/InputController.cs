﻿using UnityEngine;

public class InputController : MonoBehaviour, Observer
{
    private bool invertCameraControls = false;
    private bool launchMode = false;
    private bool waitingMode = false;
    private bool inputDisabled = false;
    private float launchTime = 0f;
    private Vector2 oldPoint;

    public float cameraRotateSpeed = 4000f;
    public float launchBuffer = 100f;
    public float launchTimeBuffer = 5f;
    public Camera cam;
    public BehindCamera behindCamera;
    public PlayerController player;
    public FuelController fuel;

    public Collider hitboxCollider;
    
    public Transform playerTransform;
    public Transform playerPitchTransform;

    private void Start()
    {

    }

    private void Awake()
    {

    }

    private void Update()
    {
        // Quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // If input is disabled, stop.
        if (inputDisabled)
        {
            return;
        }

        // See if player was tapped. If he was, set launchMode and waitingMode to true, otherwise false.
        if (Input.GetMouseButtonDown(0))
        {
            launchMode = DetectPlayerTap();
            if (launchMode)
            {
                waitingMode = true;
                launchTime = Time.timeSinceLevelLoad;
                oldPoint = Input.mousePosition;
            }
        }

        if (waitingMode)
        {
            // If in waiting mode, handle waiting mode
            HandleWaitingMode();
        }
        else if (launchMode && fuel.HasFuel())
        {
            // If in launch mode and we have fuel, handle launch mode
            HandleLaunchMode();
        }
        else if (fuel.HasFuel())
        {
            // If not in launch mode and we have fuel, handle camera mode
            HandleCameraMode();
        }

    }

    private void HandleWaitingMode()
    {
        if (Input.GetMouseButtonUp(0))
        {
            waitingMode = false;
            launchMode = false;
        }
        // If in waiting mode and button hasn't been released after the buffer time, commit to the launch
        else if (launchTime + launchTimeBuffer < Time.timeSinceLevelLoad)
        {
            waitingMode = false;
        }
    }

    private void HandleLaunchMode()
    {
        // Rotate player pitch so it faces camera direction and update velocity meter according to where finger is on the screen
        if (Input.GetMouseButton(0))
        {
            playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;

            // TODO: Implement an UIController that can handle updating the UI with method calls,
            //       so we aren't updating this part of the UI every frame... /Malte
            player.SetLaunchForce(GetLaunchForce());
        }

        // Launch 
        if (Input.GetMouseButtonUp(0))
        {
            var evt = new ObserverEvent(EventName.PlayerLaunch);

            evt.payload.Add(PayloadConstants.LAUNCH_SPEED, GetLaunchForce());
            Subject.instance.Notify(gameObject, evt);

            launchMode = false;
        }
    }

    private void HandleCameraMode()
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
        }
    }

    // Returns true if player was tapped, otherwise false.
    private bool DetectPlayerTap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return hitboxCollider.Raycast(ray, out hit, 1000f);
    }

    // Calculate force from old mouse position and current mouse position
    private float GetLaunchForce()
    {
        float difference = oldPoint.y - Input.mousePosition.y;
        float maxDifference = oldPoint.y - launchBuffer;

        return (difference / maxDifference).Clamp(0f, 1f);
    }

    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

    private Vector2 ScreenCenter()
    {
        return new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f);
    }

    private void DirectedRotation(Vector2 offset)
    {
        float xScale = behindCamera.pitch.transform.up.y;
        behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
        behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerDead:
                inputDisabled = true;
                break;
            case EventName.ToggleCameraControls:
                invertCameraControls = !invertCameraControls;
                break;
            default:
                break;
        }
    }
}