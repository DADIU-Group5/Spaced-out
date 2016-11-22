using UnityEngine;
using System.Collections;

public class ParticleSpawner : MonoBehaviour, Observer {

    //Dave
    public GameObject charge;
    public GameObject electro;
    public GameObject onFire;
    public GameObject wallHit;
    public GameObject launch;
    public GameObject respawn;

    //Objects
    public GameObject explodingBarrel;
    public GameObject malfunctioningDoor;
    public GameObject pickup;

    private Transform playerpos;

    private bool playerspawned = false;

    void Start()
    {
        Subject.instance.AddObserver(this);
    }

	public void OnNotify(GameObject go, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.Collision:
                if ((bool)evt.payload[PayloadConstants.COLLISION_STATIC])
                {
                    SpawnParticles(wallHit, (Vector3)evt.payload[PayloadConstants.POSITION]);
                }
                break;

            case EventName.PlayerLaunch:
                SpawnParticles(launch, go.transform.position, true);
                break;

            case EventName.PlayerFuelPickup:
                SpawnParticles(pickup, go.transform.position);
                break;

            case EventName.PlayerSpawned:
                playerpos = ((GameObject)evt.payload[PayloadConstants.PLAYER]).transform;
                if (playerspawned)
                {
                    SpawnParticles(respawn, playerpos.position, true);
                }
                else
                {
                    playerspawned = true;
                }
                break;

            case EventName.OnFire:
                SpawnParticles(onFire, playerpos.position, true);
                break;

            case EventName.Electrocuted:
                SpawnParticles(electro, playerpos.position, true);
                break;

            case EventName.BarrelTriggered:
                break;

            case EventName.BarrelExplosion:
                break;

            case EventName.ComicPickup:
                SpawnParticles(pickup, go.transform.position);
                break;

            default:
                break;
        }
    }

    void SpawnParticles(GameObject type, Vector3 pos, bool playerAsParent = false)
    {
        if (playerAsParent)
        {
            Instantiate(type, pos, playerpos.rotation, playerpos);
        }
        else
        {
            Instantiate(type, pos, Quaternion.identity);
        }
    }

    void OnDestroy()
    {
        if (Subject.instance != null)
        {
            Subject.instance.RemoveObserver(this);
        }
    }
}
