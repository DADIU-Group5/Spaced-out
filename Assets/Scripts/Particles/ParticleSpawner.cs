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
    public GameObject dead;

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
                if(playerpos == null)
                {
                    playerpos = go.transform;
                }
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

            case EventName.PlayerExploded:
            case EventName.OnFire:
                SpawnParticles(onFire, playerpos.position, true);
                break;

            case EventName.Electrocuted:
                SpawnParticles(electro, playerpos.position, true);
                break;

            case EventName.BarrelExplosion:
                SpawnParticles(explodingBarrel, go.transform.position);
                break;

            case EventName.PickUpKey:
            case EventName.ComicPickup:
                SpawnParticles(pickup, go.transform.position);
                break;

            case EventName.PlayerDeadEffect:
                SpawnParticles(dead, playerpos.position, true);
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

    public void OnDestroy()
    {
        if (Subject.instance != null)
        {
            Subject.instance.RemoveObserver(this);
        }
    }
}
