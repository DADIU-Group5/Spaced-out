using UnityEngine;
using System.Collections;

public class InnerCornorSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetInnerCornors();
        base.LoadObjects();
    }
}
