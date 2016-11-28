using UnityEngine;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    public GameObject muteBtn;
    public GameObject unmuteBtn;
    public Toggle cameraControls;

    void Start()
    {
        // listens to volume slider changes
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        // setup mute / unmute buttons
        var isMuted = SettingsManager.instance.settings.mute;
        muteBtn.SetActive(!isMuted);
        unmuteBtn.SetActive(isMuted);

        cameraControls.isOn = SettingsManager.instance.settings.invertedCamera;

        SetUpSound();
    }

    private void SetUpSound()
    {
        var settings = SettingsManager.instance.settings;
        var soundManager = SoundManager.instance;

        masterSlider.value = settings.masterVolume;
        musicSlider.value = settings.musicVolume;
        effectsSlider.value = settings.effectsVolume;

        soundManager.SetMasterVolume(settings.masterVolume);
        soundManager.SetMusicVolume(settings.musicVolume);
        soundManager.SetEffectsVolume(settings.effectsVolume);

        SettingsManager.instance.MuteSound(settings.mute);
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

    public void OnMuteBtnClick(bool mute)
    {
        muteBtn.SetActive(!mute);
        unmuteBtn.SetActive(mute);
        SettingsManager.instance.MuteSound(mute);
    }

    public void OnEnglishClick()
    {
        SettingsManager.instance.SetLanguage(Language.English);
    }

    public void OnDanishClick()
    {
        SettingsManager.instance.SetLanguage(Language.Danish);
    }

    public void OnCameraInvertedToggle()
    {
        SettingsManager.instance.settings.invertedCamera = !SettingsManager.instance.settings.invertedCamera;

        var evt = new ObserverEvent(EventName.ToggleCameraControls);
        Subject.instance.Notify(gameObject, evt);
    }
}
