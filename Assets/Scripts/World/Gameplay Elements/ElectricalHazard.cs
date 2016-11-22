using UnityEngine;
using System.Collections;

public class ElectricalHazard : MonoBehaviour {

    public float TimeUntilShockToDeath = 0f;

    [HideInInspector]
    public GameObject player;
    private bool shockingPlayer = false;
    [HideInInspector]
    public GameplayElement itemState;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && itemState.On && !shockingPlayer)
        {
            //set shockingplayer to true, to avoid multiple notifies (mostly for subtitles/voices, etc.)
            shockingPlayer = true;
            StartCoroutine(ShockToDeath());
        }
    }

    //if player exits trigger, reset player detection
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && itemState.On)
        {
            shockingPlayer = false;
        }
    }

    /// <summary>
    /// Shocks the player to death.
    /// </summary>
    public IEnumerator ShockToDeath()
    {
        yield return new WaitForSeconds(TimeUntilShockToDeath);
        //player.GetComponent<PlayerController>().Kill();
        var evt = new ObserverEvent(EventName.Electrocuted);
        Subject.instance.Notify(gameObject, evt);
    }
}
