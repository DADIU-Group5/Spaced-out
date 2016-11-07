using UnityEngine;
using System.Collections;

public class StaticObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetStaticObjects();
        base.LoadObjects();
    }
}
