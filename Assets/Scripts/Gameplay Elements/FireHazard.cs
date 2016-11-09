using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FireHazard : MonoBehaviour {

    //public float TimeUntilBurnToDeath = 20f;

    [HideInInspector]
    public GameObject player;
    private bool burningPlayer = false;
    public bool extinquishFlames = false;

    public Text burningText;

    [HideInInspector]
    public GameplayElement itemState;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !burningPlayer && itemState.On)
        {
            burningPlayer = true;
           // player.GetComponent<PlayerController>().onFire = true;
            var evt = new ObserverEvent(EventName.OnFire);
            Subject.instance.Notify(gameObject, evt);

            burningText.text = "BURNING!";
            //StartCoroutine(BurnToDeath());
        }
    }

    /// <summary>
    /// Burn the player to death, if fire is not put out.
    /// </summary>
    /*public IEnumerator BurnToDeath()
    {
        yield return new WaitForSeconds(TimeUntilBurnToDeath);

        //if the player is still on fire after this time, die.
        if (player.GetComponent < PlayerBehaviour >(). onFire)
        {
            Debug.Log("Player has burned to death!");
            
        }
    }*/
}
