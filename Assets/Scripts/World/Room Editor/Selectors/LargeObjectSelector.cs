using UnityEngine;
using System.Collections;

public class LargeObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetLarge();
        base.LoadObjects();
    }
}
