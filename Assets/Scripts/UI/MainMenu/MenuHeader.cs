using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuHeader : MonoBehaviour {

    public Text medalLabel;

    public void Start()
    {
        UpdateMedalLabel();
    }

	public void UpdateMedalLabel()
    {
        medalLabel.text = ProgressManager.instance.GetCurrency().ToString();
    }

    public void ResetMedals()
    {
        ProgressManager.instance.ResetCurrency();
        medalLabel.text = "0";
    }

    public void ResetAll()
    {
        ProgressManager.instance.Reset();
        GenerationDataManager.instance.Reset();
        medalLabel.text = "0";
    }
}
