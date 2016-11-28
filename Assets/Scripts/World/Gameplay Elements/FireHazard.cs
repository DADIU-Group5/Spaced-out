using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FireHazard : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    public bool extinquishFlames = false;
    public float timeBeforeResetting = 2f;

    [HideInInspector]
    public GameplayElement itemState;

    //keep track of whether the player is burning...
    //so that we don't send multiple burn notifications
    //to scripts that can't handle it (GAL lines, ex.)
    private bool burningPlayer = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        itemState = this.gameObject.GetComponent<GameplayElement>();
    }

    public IEnumerator ShockToDeath()
    {
        yield return new WaitForSeconds(timeBeforeResetting);
        burningPlayer = false;
    }


        void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && itemState.On && !burningPlayer)
        {
            burningPlayer = true;
            var evt = new ObserverEvent(EventName.OnFire);
            Subject.instance.Notify(gameObject, evt);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            burningPlayer = false;
        }
    }
}
