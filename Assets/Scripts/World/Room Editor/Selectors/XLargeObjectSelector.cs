using UnityEngine;
using System.Collections;

public class XLargeObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetXLarge();
        base.LoadObjects();
    }
}
