using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class InputController : MonoBehaviour
{
    //private bool primed = false;
    private bool invertCameraControls = false;
    private Vector2 oldPoint;
    //private Camera current;
    //private Plane inputPlane;

    //public bool tilt = false
    //public bool sidescroll = false, swipeRotate = true;

    public float cameraRotateSpeed = 4000f;
    //public Camera camera;
    public BehindCamera behindCamera;
    public PlayerController player;
    public Transform playerTransform;
    public Transform playerPitchTransform;

    public List<string> controlNames;

    public Text controlText;

    public Text chargeText;
    public Transform chargeArrow;

    public float forceScalling = 6.0f;

    private void Awake()
    {
        //inputPlane = new Plane(Vector3.right, Vector3.zero);
    }

    private void Update()
    {
        // Quit
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        //current = behind;
        //behind.enabled = true;

        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            oldPoint = Input.mousePosition;
        }

        // Look
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButtonUp(1))
        {
            Vector2 pos = Input.mousePosition;
            Vector2 offset;
            //  if (swipeRotate)
            //  {
            offset = pos - oldPoint;
            //  }
            //else
            // {
            //    offset = pos - ScreenCenter();
            //}

            if (invertCameraControls)
            {
                offset = -offset;
            }
            DirectedRotation(offset);//, tilt);
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

            player.SetLaunchForce(GetLaunchForce());
        }

        // Launch
        if (Input.GetMouseButtonUp(1))
        {
            player.GetComponent<Rigidbody>().freezeRotation = false;

            player.Launch(GetLaunchForce());

            oldPoint = Input.mousePosition;
        }
    }

    private float GetLaunchForce()
    {
        Vector2 difference = oldPoint - (Vector2)Input.mousePosition;

        float launchForce = difference.y * forceScalling;

        return launchForce.Clamp(player.minLaunchForce, player.maxLaunchForce);
    }

    private void ResetRotation()
    {
        playerTransform.rotation = Quaternion.identity;
        playerPitchTransform.rotation = Quaternion.identity;
    }

    private Vector2 ScreenCenter()
    {
        var cam = GetComponent<Camera>();
        return new Vector2(cam.pixelWidth / 2f, cam.pixelHeight / 2f);
    }

    private void DirectedRotation(Vector2 offset)
    {
        float xScale = behindCamera.pitch.transform.up.y;
        behindCamera.transform.Rotate(Vector3.up, Time.deltaTime * xScale * cameraRotateSpeed * (offset.x / ScreenCenter().magnitude));
        behindCamera.pitch.transform.Rotate(Vector3.right, Time.deltaTime * cameraRotateSpeed * (-offset.y / ScreenCenter().magnitude));
    }

    public void ToggleCameraControls()
    {
        invertCameraControls = !invertCameraControls;
    }
}