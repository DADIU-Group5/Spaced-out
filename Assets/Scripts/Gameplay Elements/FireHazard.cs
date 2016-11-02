using UnityEngine;
using System.Collections;

public class FireHazard : MonoBehaviour {

    public float TimeUntilBurnToDeath = 20f;
    public GameObject player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
    public IEnumerator BurnToDeath()
    {
        yield return new WaitForSeconds(TimeUntilBurnToDeath);

        //if the player is still on fire after this time, die.
        if (player.GetComponent < PlayerBehaviour >(). onFire)
        {
            Debug.Log("Player has burned to death!");
        }
    }
}
