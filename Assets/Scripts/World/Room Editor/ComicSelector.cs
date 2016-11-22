using UnityEngine;
using System.Collections;

public class ComicSelector : ObjectSelector
{

    public override void LoadObjects()
    {
        canBe = ObjectDatabase.instance.GetComic();
        base.LoadObjects();
    }
}
