using UnityEngine;
using System.Collections;

public class EpilogueKey : MonoBehaviour {

    public float pickUpAnimationTime  = 1.09f;
    public SpaceZoom spaceZoomScript;
    private GameObject player;
    private bool playerPickedMeUp = false;
    private bool atPosition = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !playerPickedMeUp)
        {
            //TODO: sound. "I will open the door now." (or whatever).

            playerPickedMeUp = true;

            player = other.gameObject;

            StartCoroutine(PlayPickUpAnimation());

            var evt = new ObserverEvent(EventName.EPILOGUE_EVENTONE);
            Subject.instance.Notify(other.gameObject, evt);
        }
    }

    void Update()
    {
        if (playerPickedMeUp && !atPosition)
        {
            float step = 5f * Time.deltaTime;
            Vector3 movePos = player.transform.position;
            movePos.y = gameObject.transform.position.y;
            player.transform.position = Vector3.MoveTowards(player.transform.position, gameObject.transform.position, step);

            if (Mathf.Abs (player.transform.position.y) - Mathf.Abs(gameObject.transform.position.y) < 2f)
            {
                atPosition = true;
                
            }
        }
    }

    IEnumerator PlayPickUpAnimation()
    {
        player.GetComponentInChildren<Animator>().SetTrigger("Pick Up");
        
        yield return new WaitForSeconds(pickUpAnimationTime) ;

        spaceZoomScript.StartZooming();
    }
}
