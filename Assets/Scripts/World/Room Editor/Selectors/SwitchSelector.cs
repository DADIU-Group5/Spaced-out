using UnityEngine;
using System.Collections;

public class SwitchSelector : ObjectSelector
{

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetSwitch();
        base.LoadObjects();
    }
}
