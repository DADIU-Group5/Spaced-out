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
        var isMuted = SettingsManager.instance.IsMute();
        muteBtn.SetActive(!isMuted);
        //unmuteBtn.SetActive(isMuted);

        cameraControls.isOn = SettingsManager.instance.GetInvertedCamera();

        SetupSoundSliders();
    }

    private void SetupSoundSliders()
    {
        var soundManager = SoundManager.instance;

        masterSlider.value =  SettingsManager.instance.GetMasterVolume();
        musicSlider.value = SettingsManager.instance.GetMusicVolume();
        effectsSlider.value = SettingsManager.instance.GetEffectsVolume();
        //muteBtn
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
        SettingsManager.instance.SetInvertedCamera(cameraControls.isOn);

        var evt = new ObserverEvent(EventName.ToggleCameraControls);
        Subject.instance.Notify(gameObject, evt);
    }
}
