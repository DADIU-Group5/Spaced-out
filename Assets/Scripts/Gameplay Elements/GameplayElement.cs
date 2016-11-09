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
    none, automaticDoors, switchItem, hazardSwitch, sticky, bouncy, slippery, fire,
    ice, grabby, electrocution, explosive, fan
}

public enum ItemSize
{
    small, medium, grande
}

public enum Fragility
{
    fragile, solid
}

public class GameplayElement : MonoBehaviour {

    public Behaviour behaviour = Behaviour.none;
    public Movement movement = Movement.none;
    public ItemSize itemSize = ItemSize.small;
    public Fragility fragility = Fragility.fragile;
    [HideInInspector]
    public bool On = true;

    /// <summary>
    /// Set Fragility
    /// </summary>
    public void SetFragility(Fragility _fragility)
    {
        fragility = _fragility;
    }

    /// <summary>
    /// Set Behaviour
    /// </summary>
    public void SetBehaviour(Behaviour _behaviour)
    {
        behaviour = _behaviour;
    }

    /// <summary>
    /// Set Movement 
    /// </summary>
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

    /// <summary>
    /// Set Size
    /// </summary>
    public void SetSize(ItemSize _itemSize)
    {
        itemSize = _itemSize;
    }

}
