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
            case Behaviour.bouncy:
                Debug.Log("Bouncy Castle!");
                return;
            case Behaviour.slippery:
                Debug.Log("Slippery like and eel!");
                return;
            case Behaviour.sticky:
                Debug.Log("What a sticky situation.");
                return;
            case Behaviour.explosive:
                Debug.Log("Boom! Explosive.");
                return;
            case Behaviour.electrocution:
                Debug.Log("Hair-raising!");
                return;
            default:
                return;


        }


    }
}
