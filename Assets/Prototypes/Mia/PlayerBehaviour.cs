using UnityEngine;
using System.Collections;

// my thought was that this script could be used for
// collisions, and player behaviour.
// input/movement would be a separate script.
public class PlayerBehaviour : MonoBehaviour {



    Rigidbody rgb;

    void Start()
    {
        rgb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
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
                return;
            case Behaviour.bouncy:
                Debug.Log("Bouncy!");
                return;
            case Behaviour.slippery:
                Debug.Log("Slippery.");
                return;
            /*case Behaviour.sticky:
                Debug.Log("Slippery.");
                return;*/
            case Behaviour.explosive:
                Debug.Log("Boom! Explosive.");
                return;
            case Behaviour.electrocution:
                Debug.Log("Electrocution!");
                return;
            default:
                return;


        }


    }
}
