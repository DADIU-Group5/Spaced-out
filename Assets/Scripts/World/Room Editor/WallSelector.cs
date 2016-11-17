using UnityEngine;
using System.Collections;

public class WallSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetWalls();
        base.LoadObjects();
    }
}
