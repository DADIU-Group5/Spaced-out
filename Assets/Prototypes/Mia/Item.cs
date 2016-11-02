using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//anything else?
public enum Movement
{
    none, staticItem, floatingItem
}

//just added some for example.
public enum Behaviour
{
    none, sticky, bouncy, slippery, fire,
    ice, grabby, electrocution, explosive
}

public class Item : MonoBehaviour {

    public Behaviour behaviour = Behaviour.none;
    public Movement movement = Movement.none;

    public void SetBehaviour(Behaviour _behaviour)
    {
        behaviour = _behaviour;
    }

    public void SetMovement(Movement _movement)
    {
        movement = _movement;
    }

}
