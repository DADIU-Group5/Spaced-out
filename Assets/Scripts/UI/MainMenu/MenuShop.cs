using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour {
    
    public void OnBuy50Click()
    {
        ProgressManager.instance.ChangeStars(50);
    }

    public void OnBuy20Click()
    {
        ProgressManager.instance.ChangeStars(20);
    }
}
