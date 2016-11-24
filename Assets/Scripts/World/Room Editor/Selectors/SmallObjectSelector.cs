using UnityEngine;
using System.Collections;

public class SmallObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetSmall();
        base.LoadObjects();
    }
}
