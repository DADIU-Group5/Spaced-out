using UnityEngine;
using System.Collections;

public class BehindCamera : MonoBehaviour, Observer
{
    public float angle = 0.1f, distance = 4;
    public GameObject target, pod, pitch, cam;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
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
        }
    }
}
