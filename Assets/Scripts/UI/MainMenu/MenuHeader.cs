﻿using UnityEngine;
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
        if (medaltext != null)
            medaltext.text = Translator.instance.Get("medals") + ":";//medaltext.text.ToString());
        //hardcode
        if (resetMedalstext != null)
            resetMedalstext.text = Translator.instance.Get("reset medals");
        if (resetAlltext != null)
            resetAlltext.text = Translator.instance.Get("reset all");
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
