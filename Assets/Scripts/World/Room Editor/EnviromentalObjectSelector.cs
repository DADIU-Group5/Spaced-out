using UnityEngine;
using System.Collections;

public class EnviromentalObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetEnviromentalObjects();
        base.LoadObjects();
    }
}
