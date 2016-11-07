using UnityEngine;
using System.Collections;

public class FloatingObjectSelector : ObjectSelector {

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetFloatingObjects();
        base.LoadObjects();
    }
}
