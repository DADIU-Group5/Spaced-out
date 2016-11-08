using UnityEngine;
using System.Collections;

public class ElectricalHazard : MonoBehaviour {

    public float TimeUntilShockToDeath = 0f;

    [HideInInspector]
    public GameObject player;
    private bool shockingPlayer = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !shockingPlayer)
        {
            shockingPlayer = true;
            player.GetComponent<PlayerController>().onFire = true;
            StartCoroutine(ShockToDeath());
        }
    }

    /// <summary>
    /// Burn the player to death, if fire is not put out.
    /// </summary>
    public IEnumerator ShockToDeath()
    {
        yield return new WaitForSeconds(TimeUntilShockToDeath);

        //if the player is still on fire after this time, die.
        if (player.GetComponent<PlayerController>().onFire)
        {
            Debug.Log("Player was electrocuted!");
            player.GetComponent<PlayerController>().Kill();
        }
    }
}
