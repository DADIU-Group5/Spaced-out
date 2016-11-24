using UnityEngine;
using System.Collections;

public class EnviromentalObjectSelector : ObjectSelector {

    void Start()
    {
        Debug.Log("Is this still used? " + gameObject.name);
    }

    /*public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetEnviromentalObjects();
        base.LoadObjects();
    }*/
}
