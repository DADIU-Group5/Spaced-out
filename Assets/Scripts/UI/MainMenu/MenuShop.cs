using UnityEngine;
using System.Collections;

public class MenuShop : MonoBehaviour {

    public void OnBuy50Click()
    {
        ProgressManager.instance.BuyCurrency(50);
    }

    public void OnBuy20Click()
    {
        ProgressManager.instance.BuyCurrency(20);
    }
}
