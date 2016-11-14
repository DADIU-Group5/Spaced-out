﻿using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    public Text muteBtnTxt;
    public Text englishBtnTxt;
    public Text danishBtnTxt;
    public Text resetProgBtnTxt;
    public Text disableNotifyBtnTxt;
    public Text creditsBtnTxt;
    public Text backBtnTxt;

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(SettingsManager.instance.settings.language);

        SetUpSound();
    }

    void OnDestroy()
    {
        SettingsManager.instance.onLanguageChanged -= UpdateButtonText;
    }

    public void SetUpSound()
    {
        var settings = SettingsManager.instance.settings;
        var soundManager = SoundManager.instance;

        masterSlider.value = settings.masterVolume;
        musicSlider.value = settings.musicVolume;
        effectsSlider.value = settings.effectsVolume;

        soundManager.SetMasterVolume(settings.masterVolume);
        soundManager.SetMusicVolume(settings.musicVolume);
        soundManager.SetEffectsVolume(settings.effectsVolume);
    }

    public void OnMasterSliderChanged(float value)
    {
        SettingsManager.instance.SetMasterVolume(value);
    }

    public void OnMusicSliderChanged(float value)
    {
        SettingsManager.instance.SetMusicVolume(value);
    }

    public void OnEffectsSliderChanged(float value)
    {
        SettingsManager.instance.SetEffectsVolume(value);
    }

    public void OnMuteClick()
    {
        // TODO: kinda retarted, refactor
        if(AudioListener.volume == 0)
            SettingsManager.instance.SetMasterVolume(SettingsManager.instance.settings.masterVolume);
        else
            SettingsManager.instance.SetMasterVolume(0);
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
