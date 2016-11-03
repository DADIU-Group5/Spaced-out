﻿using UnityEngine;
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
    none, switchItem, hazardSwitch, sticky, bouncy, slippery, fire,
    ice, grabby, electrocution, explosive
}

public enum ItemSize
{
    small, medium, grande
}

public class Item : MonoBehaviour {

    public Behaviour behaviour = Behaviour.none;
    public Movement movement = Movement.none;
    public ItemSize itemSize = ItemSize.small;

    public void SetBehaviour(Behaviour _behaviour)
    {
        behaviour = _behaviour;
    }

    public void SetMovement(Movement _movement)
    {
        movement = _movement;
        //if this item floats, then it needs a rigidbody to be manipulated by hazards.
        /*if (movement == Movement.floatingItem)
        {
            this.gameObject.AddComponent<Rigidbody>();
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }*/
    }

    public void SetSize(ItemSize _itemSize)
    {
        itemSize = _itemSize;
    }

}
