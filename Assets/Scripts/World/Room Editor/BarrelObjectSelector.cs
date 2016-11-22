using UnityEngine;
using System.Collections;

public class BarrelObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetBarrel();
        base.LoadObjects();
    }
}
