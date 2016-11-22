using UnityEngine;
using System.Collections;

public class FuelSelector : ObjectSelector
{

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetFuel();
        base.LoadObjects();
    }
}
