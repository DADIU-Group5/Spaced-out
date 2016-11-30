using UnityEngine;
using System;

public interface ICameraController
{
    void SetViewDirection(Vector3 direction);
}

/// <summary>
/// This class is responsible for controlling the camera.
/// It should not affect the player in any way, only read properties
/// </summary>
public class CameraController : MonoBehaviour, ICameraController, Observer
{
    public GameObject target, pod, pitch, cam;
    
    public float zoomDuration = 1f;
    public float camZoomOutPosition = -2f;
    public float camZoomInPosition = -0.9f;
    public float bufferRadius = 0.2f;
    
    private enum Zoom { NONE, IN, OUT };
    private Zoom zooming = Zoom.NONE;

    private Vector3 camZoomOut, camZoomIn, camZoomCurrent, camZoomStartPos;
    private float zoomCurrent = 0f;
    
    private Vector3 direction;
    private RaycastHit hit;

    private bool extraZoom = false;
    
    private void Awake()
    {
        Subject.instance.AddObserver(this);
        
        camZoomOut = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camZoomOutPosition);
        camZoomIn = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camZoomInPosition);
        camZoomCurrent = camZoomIn;
        camZoomStartPos = camZoomIn;
    }

    // First do zooming in Update(), then place camera in front of objects if the current zoom position is incorrect
    void Update()
    {
        // First, check if there is a target. If not, don't do anything.
        if (!target)
        {
            return;
        }

        // Set position of pod to player position.
        pod.transform.position = target.transform.position;
        
        // Perform the zooming of the camera if needed.
        switch (zooming)
        {
            case Zoom.OUT:
                PerformCameraZoom(camZoomStartPos, camZoomOut);
                break;
            case Zoom.IN:
                PerformCameraZoom(camZoomStartPos, camZoomIn);
                break;
            case Zoom.NONE:
                //camZoomCurrent = camZoomIn;
                break;
            default:
                break;
        }
        cam.transform.localPosition = camZoomCurrent;

        // Check if camera is inside an object, and if so, put it in front of the object.
        direction = cam.transform.position - target.transform.position;
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int layermask3 = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int finalmask = ~(layermask1 | layermask2 | layermask3);
        
        if (Physics.SphereCast(target.transform.position, bufferRadius, direction.normalized, out hit, direction.magnitude, finalmask))
        {
            cam.transform.position = target.transform.position + hit.distance * direction.normalized;
            if (!extraZoom)
            {
                extraZoom = true;
                var evt = new ObserverEvent(EventName.PlayerFadeValue);
                evt.payload.Add(PayloadConstants.PERCENT, 0.25f);
                Subject.instance.Notify(gameObject, evt);
            }
        }
        else
        {
            if (extraZoom)
            {
                extraZoom = false;
                var evt = new ObserverEvent(EventName.PlayerFadeValue);
                evt.payload.Add(PayloadConstants.PERCENT, 1f);
                Subject.instance.Notify(gameObject, evt);
            }
        }

        /*
        if (Physics.Raycast(target.transform.position, direction.normalized, out hit, maxDistance: direction.magnitude, layerMask: finalmask))
        {
            cam.transform.position = hit.point;
        }
        */
    }

    private void PerformCameraZoom(Vector3 start, Vector3 end)
    {
        float t = zoomCurrent / zoomDuration;

        if (zoomCurrent > zoomDuration)
        {
            if (zooming == Zoom.IN)
            {
                camZoomCurrent = camZoomIn;
                cam.transform.localPosition = camZoomCurrent;
                zooming = Zoom.NONE;
            }
        }
        else if (t < 1f)
        {
            zoomCurrent += Time.deltaTime;
            camZoomCurrent = new Vector3(Mathf.SmoothStep(start.x, end.x, t), Mathf.SmoothStep(start.y, end.y, t), Mathf.SmoothStep(start.z, end.z, t));
            cam.transform.localPosition = camZoomCurrent;
        }
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerSpawned:
                var payload = evt.payload;
                GameObject player = (GameObject)payload[PayloadConstants.PLAYER];
                target = player;
                extraZoom = false;
                break;
            case EventName.CameraZoomOut:
                zooming = Zoom.OUT;
                zoomCurrent = 0f;
                camZoomStartPos = camZoomCurrent;
                break;
            case EventName.CameraZoomIn:
                zooming = Zoom.IN;
                zoomCurrent = 0f;
                camZoomStartPos = camZoomCurrent;
                break;
            case EventName.ToggleUI:
                gameObject.SetActive(!gameObject.activeSelf);
                extraZoom = false;
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    /// <summary>
    /// Sets the direction of the camera
    /// </summary>
    /// <param name="direction">Direction to look</param>
    public void SetViewDirection(Vector3 direction)
    {
        throw new NotImplementedException();
    }
}
