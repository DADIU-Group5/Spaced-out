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
        if (other.transform.tag == "Player")
        {
            StartCoroutine(ShockToDeath());
        }
    }

    /// <summary>
    /// Burn the player to death, if fire is not put out.
    /// </summary>
    public IEnumerator ShockToDeath()
    {
        yield return new WaitForSeconds(TimeUntilShockToDeath);
        Debug.Log("Player was electrocuted!");
        //player.GetComponent<PlayerController>().Kill();
        var evt = new ObserverEvent(EventName.Electrocuted);
        Subject.instance.Notify(gameObject, evt);
    }
}
