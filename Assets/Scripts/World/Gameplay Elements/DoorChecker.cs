using UnityEngine;
using System.Collections;

public class DoorChecker : MonoBehaviour {

    public float waitToRemoveDoor = 0.5f;

    [HideInInspector]
    public bool playerFound = false;

    [HideInInspector]
    public GameObject doorParent;

    private string doorNumber;
    private bool malfunctioningDoors = false;


    //Make sure to update parent door script on how many doors are touching player
    //Player only dies when hit with two (child) doors at once.

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Player")
        {
            if (malfunctioningDoors)
            {
                doorParent.GetComponent<MalfunctioningDoors>().doorsTouchingPlayer += 1;
            } else
            {
                doorParent.GetComponent<AutomaticDoors>().doorsTouchingPlayer += 1;
            }
        }
    }

    void OnCollisionExit(Collision other)
    {

        if (other.transform.tag == "Player" )
        {
            StartCoroutine(RemovePoint());
        }
    }

    public IEnumerator RemovePoint()
    {
        yield return new WaitForSeconds(waitToRemoveDoor);
        if (malfunctioningDoors)
        {
            doorParent.GetComponent<MalfunctioningDoors>().doorsTouchingPlayer -= 1;
        }
        else
        {
            doorParent.GetComponent<AutomaticDoors>().doorsTouchingPlayer -= 1;
        }
    }

    // Use this for initialization
    void Start () {
        doorParent = gameObject.transform.parent.gameObject;
        if (doorParent.GetComponent<MalfunctioningDoors>() != null)
            malfunctioningDoors = true;
	}
	
}
