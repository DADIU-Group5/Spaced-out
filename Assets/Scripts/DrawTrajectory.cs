using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawTrajectory : MonoBehaviour, Observer {

    public LineRenderer LR;
    public float length = 1;
    public int maxBounces = 4;
    public InputController inputCont;

    private Transform target;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 direction;

    private float lengthLeft;
    private int currentBounces;
    private RaycastHit globalHit;
    private Vector3 currentRayPos, currentDirection;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }
    
    void LateUpdate () {
        points.Clear();
        lengthLeft = length;
        currentBounces = 0;
        direction = inputCont.GetLaunchDirection();
        
        AddPoint(target.position);

        currentRayPos = target.position;
        currentDirection = inputCont.GetLaunchDirection();

        while (lengthLeft > 0 && currentBounces <= maxBounces)
        {
            lengthLeft = TryAddPoint(currentRayPos, currentDirection, lengthLeft);
            currentRayPos = globalHit.point;
            currentDirection = Vector3.Reflect(currentDirection, globalHit.normal);
            currentBounces++;
        }

        Draw();
	}

    // Tries to cast a ray and adds the point that was hit. If nothing was hit, add point in space according to length variable
    // Returns the remaining length after adding the point
    private float TryAddPoint(Vector3 raypos, Vector3 dir, float len)
    {
        Ray ray = new Ray(raypos, dir);
        RaycastHit hit;
        
        // Create layermask that ignores all Golfball and Ragdoll layers
        int layermask1 = 1 << LayerMask.NameToLayer("Golfball");
        int layermask2 = 1 << LayerMask.NameToLayer("Ragdoll");
        int layermask3 = 1 << LayerMask.NameToLayer("Ignore Raycast");
        int finalmask = ~(layermask1 | layermask2 | layermask3);

        if (Physics.Raycast(ray, out hit, len, finalmask))
        {
            AddPoint(hit.point);
            globalHit = hit;
            return len - hit.distance;
        }
        else
        {
            AddPoint(raypos + (dir.normalized * len));
            globalHit = hit;
            return 0;
        }
    }

    void AddPoint(Vector3 v3)
    {
        points.Add(v3);
    }

    void Draw()
    {
        LR.SetVertexCount(points.Count);
        LR.SetPositions(points.ToArray());
    }

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.PlayerSpawned:
                var payload = evt.payload;
                GameObject player = (GameObject)payload[PayloadConstants.PLAYER];
                target = player.transform;
                break;
        }
    }
}
