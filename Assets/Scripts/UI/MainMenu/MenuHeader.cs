using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuHeader : MonoBehaviour {

    public Text medalLabel;
    public Text medaltext;
    public Text resetMedalstext;
    public Text resetAlltext;

    public void Start()
    {
        UpdateMedalLabel();
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    private void UpdateButtonText(Language lan)
    {
        medaltext.text = Translator.instance.Get("medals") + ":";//medaltext.text.ToString());
        resetMedalstext.text = Translator.instance.Get(resetMedalstext.text.ToString());
        resetAlltext.text = Translator.instance.Get(resetAlltext.text.ToString());
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
