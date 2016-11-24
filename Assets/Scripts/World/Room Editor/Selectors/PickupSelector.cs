using UnityEngine;
using System.Collections;

public class PickupSelector : ObjectSelector {

    public override void LoadObjects()
    {
        Debug.LogError("This should no longer be used. Contact Frederik if you see this message!");
        //canBe = ObjectDatabase.instance.GetPickupObjects();
        base.LoadObjects();
    }
}
