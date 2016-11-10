using UnityEngine;
using System.Collections;

public class PickupSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetPickupObjects();
        base.LoadObjects();
    }
}
