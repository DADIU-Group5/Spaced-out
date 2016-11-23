using UnityEngine;
using System.Collections;
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
    private float zoomCurrent = 0f;
    private float zoomStartPosition = 0f;

    private Vector3 direction;
    private RaycastHit hit;

    private enum Zoom {NONE, IN, OUT};
    Zoom zooming = Zoom.NONE;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    void Update()
    {

        switch(zooming)
        {
            case Zoom.OUT:
                InterpCameraZ(zoomStartPosition, camZoomOutPosition);
                break;
            case Zoom.IN:
                InterpCameraZ(zoomStartPosition, camZoomInPosition);
                break;
            case Zoom.NONE:
                cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, camZoomInPosition);
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (target)
        {
            pod.transform.position = target.transform.position;
        }
        else
        {
            return;
        }
        
        direction = cam.transform.position - target.transform.position;
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int layermask3 = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int finalmask = ~(layermask1 | layermask2 | layermask3);

        if (Physics.Raycast(target.transform.position, direction, out hit, maxDistance: direction.magnitude, layerMask: finalmask))
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.InverseTransformPoint(hit.point).z + camZoomInPosition);
        }

        /*
        //Vector3 direction = Vector3.SlerpUnclamped(-pitch.transform.forward, pitch.transform.up, angle);
        Vector3 direction = -pitch.transform.forward;
        Ray ray = new Ray(target.transform.position, direction);
        RaycastHit hit;
        int layerMask = LayerMask.NameToLayer("Wall");
        float rayDistance;
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            rayDistance = hit.distance;
        }
        else
        {
            rayDistance = distance;
        }
        cam.transform.localPosition = direction.normalized * rayDistance;
        */
    }

    private void InterpCameraZ(float start, float end)
    {
        if (zoomCurrent > zoomDuration)
        {
            zooming = Zoom.NONE;
        }
        else
        {
            zoomCurrent += Time.deltaTime;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, Mathf.Lerp(start, end, zoomCurrent / zoomDuration));
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
                break;
            case EventName.CameraZoomOut:
                zooming = Zoom.OUT;
                zoomCurrent = 0f;
                zoomStartPosition = cam.transform.localPosition.z;
                break;
            case EventName.CameraZoomIn:
                zooming = Zoom.IN;
                zoomCurrent = 0f;
                zoomStartPosition = cam.transform.localPosition.z;
                break;
            default:
                break;
        }
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
