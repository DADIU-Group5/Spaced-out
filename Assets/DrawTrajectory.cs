using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawTrajectory : MonoBehaviour, Observer {

    public LineRenderer LR;
    public float length = 10;
    Transform target;
    List<Vector3> points = new List<Vector3>();
    Vector3 direction;
    public InputController inputCont;
    public LayerMask lm;

    private void Awake()
    {
        Subject.instance.AddObserver(this);
    }

    // Update is called once per frame
    void Update () {
        points.Clear();
        direction = inputCont.GetLaunchDirection();
        AddPoint(target.position);
        Ray ray = new Ray(target.position, direction);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            points.Add(hit.point);
        }
        else
        {
            AddPoint(target.position + (direction.normalized * length));
        }
        Draw();
	}

    void AddPoint(Vector3 v3)
    {
        points.Add(v3);
    }

    void Draw()
    {
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
