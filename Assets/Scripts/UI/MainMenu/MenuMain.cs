using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuMain : Singleton<MenuMain> {

    private LevelSelectManager levelSelect;
    //public Text startBtnTxt;
    public Text levelSelectBtnTxt;
    public Text settingsBtnTxt;
    public Text shopBtnTxt;
    public Text exitBtnTxt;
    // The active view
    private GameObject view;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    public void UpdateButtonText(Language lan)
    {
        if (levelSelectBtnTxt != null && levelSelectBtnTxt.text != null && Translator.instance != null)
        {
            Debug.Log("level select btn text: " + levelSelectBtnTxt.text);
            levelSelectBtnTxt.text = Translator.instance.Get("levelSelect");
        }
        if (settingsBtnTxt != null && settingsBtnTxt.text != null)
            settingsBtnTxt.text = Translator.instance.Get("settings");
        if (shopBtnTxt != null && shopBtnTxt.text != null)
            shopBtnTxt.text = Translator.instance.Get("shop");
        if (exitBtnTxt != null && exitBtnTxt.text != null)
            exitBtnTxt.text = Translator.instance.Get("exit");
    }

    public void Back()
    {
        view.SetActive(false);
    }
}
