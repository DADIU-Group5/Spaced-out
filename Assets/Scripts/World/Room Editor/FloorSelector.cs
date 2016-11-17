using UnityEngine;
using System.Collections;

public class FloorSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetFloors();
        base.LoadObjects();
    }
}
