﻿using UnityEngine;

public class InputController : MonoBehaviour, Observer
{
    private bool invertCameraControls;
    private bool launchMode;
    private bool inputDisabled;










    private Vector2 oldPoint;

    public float cameraRotateSpeed = 4000f,
        launchBuffer = 100f;
    public Camera cam;
    public BehindCamera behindCamera;
    public PlayerController player;
    public FuelController fuel;
    
    public Transform playerTransform,
        playerPitchTransform;

    public Collider hitboxCollider;

    private void Awake()
    {
        invertCameraControls = true;
        Subject.instance.AddObserver(this);
    }

    private void Update()
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

        // Check if we are NOT in launchmode
        if (!launchMode)
        {
            HandleCameraMode();
        }
        else if (fuel.HasFuel())
        {
            HandleLaunchMode();
        }
    }



    // Interprest input as launch mode.
    private void HandleLaunchMode()
    {
        // Save starting position of tap
        if (Input.GetMouseButtonDown(0))
        {
            oldPoint = Input.mousePosition;
        }

        // Rotate player pitch so it faces camera direction and update velocity meter according to where finger is on the screen
        if (Input.GetMouseButton(0))
        {
            playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;
            
            player.SetLaunchForce(GetLaunchForce());
        }

        // Launch 
        float launchForce = GetLaunchForce();
        if (Input.GetMouseButtonUp(0))
        {
            if (launchForce > 0)
            {
                var evt = new ObserverEvent(EventName.PlayerLaunch);
                evt.payload.Add(PayloadConstants.LAUNCH_FORCE, launchForce);
                evt.payload.Add(PayloadConstants.LAUNCH_DIRECTION, GetLaunchDirection());
                Subject.instance.Notify(gameObject, evt);
            }
            launchMode = false;
        }
    }

    // Interprets input as camera mode.
    protected virtual void HandleCameraMode()
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

            // Commented out temporarily, do not remove!!!
            // Soon™
            //playerTransform.rotation = behindCamera.pitch.transform.rotation;
        }
    }

    // Returns true if player was tapped, otherwise false.
    private bool DetectPlayerTap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        return hitboxCollider.Raycast(ray, out hit, 1000f);
    }

    // Calculate force from old mouse position and current mouse position.
    private float GetLaunchForce()
    {
        float difference = oldPoint.y - Input.mousePosition.y;
        float maxDifference = oldPoint.y - launchBuffer;

        return (difference / maxDifference).Clamp(0f, 1f);
    }

    // Calculate the direction from the character position and the crosshair.
    public Vector3 GetLaunchDirection()
    {
        Ray ray = cam.ScreenPointToRay(ScreenCenter());
        RaycastHit hit;
        
        // Create layermask that ignores all Golfball and Ragdoll layers
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int layermask3 = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int finalmask = ~(layermask1 | layermask2 | layermask3);

        if (Physics.Raycast(ray, out hit, System.Int32.MaxValue, finalmask))
        //if (Physics.Raycast(ray, out hit))
        {
            return hit.point - player.transform.position;
        }
        else
        {
            return playerPitchTransform.forward;
        }
    }

    // Resets the rotation.
    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

    // Returns the pixel center of the camera.
    private Vector2 ScreenCenter()
    {
        return new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f);
    }

    // Rotates the assigned behindCamera in the direction of the offset.
    private void DirectedRotation(Vector2 offset)
    {
        float xScale = behindCamera.pitch.transform.up.y;
        xScale = Mathf.Sign(xScale);
        behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
        behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerSpawned:
                inputDisabled = false;
                GameObject go = evt.payload[PayloadConstants.PLAYER] as GameObject;
                player = go.GetComponent<PlayerController>();
                playerTransform = player.transform;
                playerPitchTransform = player.pitchTransform;
                fuel = player.fuel;
                break;
            case EventName.PlayerWon:
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