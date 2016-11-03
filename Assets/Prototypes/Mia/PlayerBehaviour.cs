using UnityEngine;
using System.Collections;

// my thought was that this script could be used for
// collisions, and player behaviour.
// input/movement would be a separate script.
public class PlayerBehaviour : MonoBehaviour {

    Rigidbody rgb;
    [HideInInspector]
    public bool onFire;
    

    void Start()
    {
        rgb = this.gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "object")
        {
            PlayerMetObject(other.gameObject);
        }
    }

    /// <summary>
    /// Player met and object, decide on proper reaction.
    /// </summary>
    public void PlayerMetObject(GameObject obj)
    {
        Behaviour objBehaviour = obj.GetComponent<Item>().behaviour;

        switch (objBehaviour)
        {
            case Behaviour.fire:
                Debug.Log("Burn, mofo!");
                onFire = true;
                obj.GetComponent<FireHazard>().StartCoroutine(
                    obj.GetComponent<FireHazard>().BurnToDeath());
                return;
            case Behaviour.electrocution:
                Debug.Log("Hair-raising!");
                return;
            default:
                return;


        }


    }
}
