using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawTrajectory : MonoBehaviour, Observer {

    public LineRenderer LR;
    public float length = 1;
    public int maxBounces = 4;
    public InputController inputCont;

    public Transform target;
    private List<Vector3> points = new List<Vector3>();

    private float lengthLeft;
    private int currentBounces;
    private RaycastHit globalHit;
    private Vector3 currentRayPos, currentDirection;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }
    
    void LateUpdate () {
        // First, check if there is a target, and if not, don't do anything.
        if(target == null)
        {
            return;
        }

        // Initialize variables used
        points.Clear();
        lengthLeft = length;
        currentBounces = 0;
        AddPoint(target.position);
        currentRayPos = target.position;
        currentDirection = inputCont.GetLaunchDirection();

        // While there is still length left of the line, or max number of bounces has not been reached
        while (lengthLeft > 0 && currentBounces <= maxBounces)
        {
            // Add next point and set lengthLeft according to how long the line drawn was.
            lengthLeft = TryAddPoint(currentRayPos, currentDirection, lengthLeft);

            // Update variables for next iteration
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

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
