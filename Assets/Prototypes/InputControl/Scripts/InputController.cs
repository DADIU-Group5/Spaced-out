using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private bool primed = false;
    private bool invertCameraControls = false;
    private Vector2 oldPoint;
    private Camera current;
    private Plane inputPlane;

    public bool tilt = false, sidescroll = false, swipeRotate = true;
    public float cameraRotateSpeed = 4000f;
    public Camera behind;
    public BehindCamera behindCamera;
    public PlayerController player;
    public Transform playerTransform, playerPitchTransform;

    public ControlMode selectedControl = ControlMode.HoldButton;

    public List<string> controlNames;

    public Text controlText;

    public Text chargeText;
    public Transform chargeArrow;
    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    private void Awake()
    {
        inputPlane = new Plane(Vector3.right, Vector3.zero);

        controlNames = new List<string>(){
            "Hold - Button",
            "Hold - Fingers",
            "Drag - Two Fingers",
            "Cross",
            "Swipe",
            "Jetpack micro corrections"
        };

        controlText.text = controlNames[(int)selectedControl];
    }

    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

    private Vector2 ScreenCenter()
    {
        return new Vector2(current.pixelWidth / 2f, current.pixelHeight / 2f);
    }

    public enum ControlMode
    {
        HoldButton,
        HoldFingers,
        SlingTheMofo,
        Cross,
        Swipe,
        JetPack
    }

    public void ChangeControlLeft()
    {
        if ((int)selectedControl <= 0)
            selectedControl = ControlMode.SlingTheMofo;
        else
            selectedControl--;

        controlText.text = controlNames[(int)selectedControl];
    }

    public void ChangeControlRight()
    {
        if ((int)selectedControl >= 2)
            selectedControl = ControlMode.HoldButton;
        else
            selectedControl++;

        controlText.text = controlNames[(int)selectedControl];
    }

    private void Update()
    {
        // Quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        switch (selectedControl)
        {
            case ControlMode.HoldButton:
                current = behind;
                behind.enabled = true;

                if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
                {
                    oldPoint = Input.mousePosition;
                    player.SetHoldControl(true);
                }

                // Look
                if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                {
                    Vector2 pos = Input.mousePosition;
                    Vector2 offset;
                    if (swipeRotate)
                    {
                        offset = pos - oldPoint;
                    }
                    else
                    {
                        offset = pos - ScreenCenter();
                    }

                    if (invertCameraControls)
                    {
                        offset = -offset;
                    }
                    DirectedRotation(offset, tilt);
                    oldPoint = pos;
                }

                break;

            case ControlMode.HoldFingers:
                current = behind;
                behind.enabled = true;

                if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
                {
                    oldPoint = Input.mousePosition;
                }

                // Look
                if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
                {
                    Vector2 pos = Input.mousePosition;
                    Vector2 offset;
                    if (swipeRotate)
                    {
                        offset = pos - oldPoint;
                    }
                    else
                    {
                        offset = pos - ScreenCenter();
                    }

                    if (invertCameraControls)
                    {
                        offset = -offset;
                    }
                    DirectedRotation(offset, tilt);
                    oldPoint = pos;
                }

                // Rotate player so it faces camera direction
                if (Input.GetMouseButton(1))
                {
                    player.GetComponent<Rigidbody>().freezeRotation = true;
                    playerTransform.rotation = behindCamera.transform.rotation;
                    playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;
                }

                // Move
                if (Input.GetMouseButtonDown(1))
                {
                    player.SetCharging(true);
                    player.SetHoldControl(true);
                }

                // Launch
                if (Input.GetMouseButtonUp(1))
                {
                    oldPoint = Input.mousePosition;
                    player.GetComponent<Rigidbody>().freezeRotation = false;
                    player.LaunchCharge(behindCamera.pitch.transform.forward);
                    player.SetCharging(false);
                }

                break;
            case ControlMode.SlingTheMofo:
                current = behind;
                behind.enabled = true;

                if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
                {
                    oldPoint = Input.mousePosition;
                }

                // Look
                if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
                {
                    Vector2 pos = Input.mousePosition;
                    Vector2 offset;
                    if (swipeRotate)
                    {
                        offset = pos - oldPoint;
                    }
                    else
                    {
                        offset = pos - ScreenCenter();
                    }

                    if (invertCameraControls)
                    {
                        offset = -offset;
                    }
                    DirectedRotation(offset, tilt);
                    oldPoint = pos;
                }

                // Move
                if (Input.GetMouseButtonDown(1))
                {
                    // Hide save starting positions
                    oldPoint = Input.mousePosition;
                    //player.SetCharging(true);
                    player.SetHoldControl(false);
                }

                // Rotate player so it faces camera direction
                if (Input.GetMouseButton(1))
                {
                    player.GetComponent<Rigidbody>().freezeRotation = true;
                    playerTransform.rotation = behindCamera.transform.rotation;
                    playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;
                    // Calculate distance
                    // TODO: Update power bar.
                    Vector2 difference = oldPoint - (Vector2) Input.mousePosition;
                    float launchForce = difference.y * 6;

                    if (launchForce > player.maxLaunchForce)
                        launchForce = player.maxLaunchForce;
                    if (launchForce < 0)
                        launchForce = 0;

                    player.SetLaunchForce(launchForce);
                }

                // Launch
                if (Input.GetMouseButtonUp(1))
                {
                    player.GetComponent<Rigidbody>().freezeRotation = false;
                    // Perform slingshot breh
                    // TODO: Call function for launching player.
                    Vector2 difference = oldPoint - (Vector2) Input.mousePosition;

                    float launchForce = difference.y * 6;
                    if (launchForce > player.maxLaunchForce)
                        launchForce = player.maxLaunchForce;
                    if (launchForce < 0)
                        launchForce = 0;

                    player.Launch(launchForce);

                    oldPoint = Input.mousePosition;
                }
                break;
            case ControlMode.Cross:
                break;
            case ControlMode.Swipe:
                break;
            case ControlMode.JetPack:
                break;
        }

    }


    public void OnChargeButtonPressed()
    {
        // Rotate player so it faces camera direction
        player.GetComponent<Rigidbody>().freezeRotation = true;
        playerTransform.rotation = behindCamera.transform.rotation;
        playerPitchTransform.rotation = behindCamera.pitch.transform.rotation;

        // Move
        player.SetCharging(true);
    }

    public void OnChargeButtonHeld()
    {

    }

    public void OnChargeButtonReleased()
    {
        // Launch
        player.GetComponent<Rigidbody>().freezeRotation = false;
        player.LaunchCharge(behindCamera.pitch.transform.forward);
        player.SetCharging(false);
    }

    public void DirectedRotation(Vector2 offset, bool tilt)
    {
        /*
        if (tilt)
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            playerTransform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
        else
        {
            float xScale = playerPitchTransform.up.y;
            playerTransform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            playerPitchTransform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
        */
        if (tilt)
        {
            Vector3 axis = new Vector3(-offset.y, offset.x, 0);
            behindCamera.transform.Rotate(axis, Time.deltaTime * cameraRotateSpeed * (offset.magnitude / ScreenCenter().magnitude));
        }
        else
        {
            float xScale = behindCamera.pitch.transform.up.y;
            behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
            behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
        }
    }

    public void ToggleCameraControls()
    {
        invertCameraControls = !invertCameraControls;
    }
}