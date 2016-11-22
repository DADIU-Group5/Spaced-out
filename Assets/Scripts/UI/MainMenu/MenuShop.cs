using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuShop : MonoBehaviour {

    public Text buy20Text;
    public Text buy50Text;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    private void UpdateButtonText(Language lan)
    {
        buy20Text.text = Translator.instance.Get("buy") + " " + 20 + " " + Translator.instance.Get("medals");
        buy50Text.text = Translator.instance.Get("buy") + " " + 50 + " " + Translator.instance.Get("medals");
    }

    public void OnBuy50Click()
    {
        ProgressManager.instance.ChangeCurrency(50);
    }

    public void OnBuy20Click()
    {
        ProgressManager.instance.ChangeCurrency(20);
    }
}
