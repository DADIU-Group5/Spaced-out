using UnityEngine;
using System.Collections;

public class FireHazard : MonoBehaviour {

    public float TimeUntilBurnToDeath = 20f;

    [HideInInspector]
    public GameObject player;
    private bool burningPlayer = false;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !burningPlayer)
        {
            burningPlayer = true;
            player.GetComponent<PlayerController>().onFire = true;
            StartCoroutine(BurnToDeath());
        }
    }

    /// <summary>
    /// Burn the player to death, if fire is not put out.
    /// </summary>
    public IEnumerator BurnToDeath()
    {
        yield return new WaitForSeconds(TimeUntilBurnToDeath);

        //if the player is still on fire after this time, die.
        if (player.GetComponent < PlayerController >(). onFire)
        {
            Debug.Log("Player has burned to death!");
        }
    }
}
