using UnityEngine;
using System.Collections;

public class ShapingObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetShapingObjects();
        base.LoadObjects();
    }
}
