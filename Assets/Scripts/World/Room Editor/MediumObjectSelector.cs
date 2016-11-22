using UnityEngine;
using System.Collections;

public class MediumObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetMedium();
        base.LoadObjects();
    }
}
