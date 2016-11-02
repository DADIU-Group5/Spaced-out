using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    private bool primed = false;
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

    private void Awake()
    {
        inputPlane = new Plane(Vector3.right, Vector3.zero);

        controlNames = new List<string>(){
            "Hold - Button",
            "Hold - Fingers",
            "Sling - Two Fingers",
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
            selectedControl = ControlMode.JetPack;
        else
            selectedControl--;

        controlText.text = controlNames[(int)selectedControl];
    }

    public void ChangeControlRight()
    {
        if ((int)selectedControl >= 4)
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

                if (Input.GetMouseButtonDown(0))
                {
                    oldPoint = Input.mousePosition;
                }

                // Look
                if (Input.GetMouseButton(0))
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
                    DirectedRotation(offset, tilt);
                    oldPoint = pos;
                }

                break;

            case ControlMode.HoldFingers:
                current = behind;
                behind.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    oldPoint = Input.mousePosition;
                }

                // Look
                if (Input.GetMouseButton(0))
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
                }

                // Launch
                if (Input.GetMouseButtonUp(1))
                {
                    player.GetComponent<Rigidbody>().freezeRotation = false;
                    player.LaunchCharge(behindCamera.pitch.transform.forward);
                    player.SetCharging(false);
                }

                break;
            case ControlMode.SlingTheMofo:
                current = behind;
                behind.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    oldPoint = Input.mousePosition;
                }

                // Look
                if (Input.GetMouseButton(0))
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
                }

                // Launch
                if (Input.GetMouseButtonUp(1))
                {
                    player.GetComponent<Rigidbody>().freezeRotation = false;
                    player.LaunchCharge(behindCamera.pitch.transform.forward);
                    player.SetCharging(false);
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
}