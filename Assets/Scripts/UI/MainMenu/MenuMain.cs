using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuMain : Singleton<MenuMain> {

    private LevelSelectManager levelSelect;
    public Text startBtnTxt;
    public Text levelSelectBtnTxt;
    public Text settingsBtnTxt;
    public Text shopBtnTxt;
    public Text exitBtnTxt;
    // The active view
    private GameObject view;

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(Language.Danish);
    }

    public void UpdateButtonText(Language lan)
    {
        print("Translated his");
        startBtnTxt.text = Translator.instance.Get("start");
        levelSelectBtnTxt.text = Translator.instance.Get("levelSelect");
        settingsBtnTxt.text = Translator.instance.Get("settings");
        shopBtnTxt.text = Translator.instance.Get("shop");
        exitBtnTxt.text = Translator.instance.Get("exit");
    }

    public void Back()
    {
        view.SetActive(false);
    }
}
