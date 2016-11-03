using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour {

    public Slider volumeSlider;
    public Text englishBtnTxt;
    public Text danishBtnTxt;
    public Text resetProgBtnTxt;
    public Text disableNotifyBtnTxt;
    public Text creditsBtnTxt;
    public Text backBtnTxt;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    void OnDestroy()
    {
        SettingsManager.instance.onLanguageChanged -= UpdateButtonText;
    }

    public void OnEnglishClick()
    {
        SettingsManager.instance.SetLanguage(Language.English);
    }

    public void OnDanishClick()
    {
        SettingsManager.instance.SetLanguage(Language.Danish);
    }


    private void UpdateButtonText(Language lan)
    {
        resetProgBtnTxt.text = Translator.instance.Get("resetProg");
        disableNotifyBtnTxt.text = Translator.instance.Get("disableNotify");
        creditsBtnTxt.text = Translator.instance.Get("credits");
        englishBtnTxt.text = Translator.instance.Get("english");
        danishBtnTxt.text = Translator.instance.Get("danish");
        backBtnTxt.text = Translator.instance.Get("back");
    }
}
