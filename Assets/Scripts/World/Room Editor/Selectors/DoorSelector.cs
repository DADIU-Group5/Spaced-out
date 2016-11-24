using UnityEngine;
using System.Collections;

public class DoorSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetDoors();
        base.LoadObjects();
    }
}
