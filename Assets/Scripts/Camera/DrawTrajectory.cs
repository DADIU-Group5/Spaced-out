using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour, Observer
{
    public float length = 1;
    public int maxBounces = 4;
    public InputController inputCont;

    public Transform target;
    private List<Vector3> points = new List<Vector3>();

    private float lengthLeft;
    private int currentBounces;
    private RaycastHit globalHit;
    private Vector3 currentRayPos, currentDirection;
    private LineRenderer LR;

    private bool drawingEnabled;
    private bool uiDisabled = false;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
        LR = GetComponent<LineRenderer>();
        drawingEnabled = true;
    }

    // This can only be put into Update() if it is called after CameraController's Update, since this code depends on that code to be finished
    void LateUpdate()
    {
        if (drawingEnabled)
        {
            // First, check if there is a target, and if not, don't do anything.
            if (target == null)
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
            case EventName.ToggleUI:
                points.Clear();
                Draw();
                uiDisabled = !uiDisabled;
                drawingEnabled = !uiDisabled;
                break;
            case EventName.PlayerLaunch:
                points.Clear();
                Draw();
                drawingEnabled = false;
                break;
            case EventName.PlayerReadyForLaunch:
                if (!uiDisabled)
                {
                    drawingEnabled = true;
                }
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }
}
