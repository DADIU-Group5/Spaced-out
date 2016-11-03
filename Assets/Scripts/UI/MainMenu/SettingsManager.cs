using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager> {

    public GameSettings settings;
    public Slider volumeSlider;

    public void OnMuteToggle()
    {
        settings.muted = !settings.muted;
        if (settings.muted)
        {
            volumeSlider.value = 0;
            AudioListener.volume = 0;
        }
        else
        {
            volumeSlider.value = settings.volume;
            AudioListener.volume = settings.volume / 100f;
        }

    }

	public void OnVolumeValueChanged()
    {
        settings.volume = volumeSlider.value;
        AudioListener.volume = settings.volume / 100f;
    }

    public void OnLanguageSelect(Language language)
    {
        settings.language = language;
        Translator.instance.SetLanguage(language);
    }
}
