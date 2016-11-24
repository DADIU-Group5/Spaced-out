using UnityEngine;
using System.Collections;

public class OuterCornorSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetOuterCornors();
        base.LoadObjects();
    }
}
