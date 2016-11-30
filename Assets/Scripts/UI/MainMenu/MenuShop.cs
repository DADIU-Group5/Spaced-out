using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour {
    
    public void OnBuy50Click()
    {
        ProgressManager.instance.ChangeCurrency(50);
    }

    public void OnBuy20Click()
    {
        ProgressManager.instance.ChangeCurrency(20);
    }
}
