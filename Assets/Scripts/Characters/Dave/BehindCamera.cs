using UnityEngine;
using System.Collections;

public class BehindCamera : MonoBehaviour, Observer
{
    public GameObject target, pod, pitch, cam;
    
    public float zoomDuration = 1f;
    public float camZoomOutPosition = -2f;
    public float camZoomInPosition = -0.9f;
    private float zoomCurrent = 0f;
    private float zoomStartPosition = 0f;

    private enum Zoom {NONE, IN, OUT};
    Zoom zooming = Zoom.NONE;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    void Update()
    {
        if (zooming == Zoom.OUT)
        {
            InterpCameraZ(zoomStartPosition, camZoomOutPosition);
        }
        else if (zooming == Zoom.IN)
        {
            InterpCameraZ(zoomStartPosition, camZoomInPosition);
        }
    }

    private void InterpCameraZ(float a, float b)
    {
        if (zoomCurrent > zoomDuration)
        {
            zooming = Zoom.NONE;
        }
        else
        {
            zoomCurrent += Time.deltaTime;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, Mathf.Lerp(a, b, zoomCurrent / zoomDuration));
        }
    }

    private void LateUpdate()
    {
        if (target)
        {
            pod.transform.position = target.transform.position;
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
}
