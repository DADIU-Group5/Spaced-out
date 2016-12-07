using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour {

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;
    public Slider sensitivitySlider;
    public Toggle mute;
    public Toggle cameraControls;
    public GameObject danish;
    public GameObject english;
    public GameObject danishCredits;
    public GameObject englishCredits;

    void Start()
    {
        // listens to volume slider changes
        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        // setup mute / unmute buttons
        mute.isOn = SettingsManager.instance.IsMute();
        cameraControls.isOn = SettingsManager.instance.GetInvertedCamera();
        sensitivitySlider.value = SettingsManager.instance.GetSensitivity();

        SetupSoundSliders();
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (SettingsManager.instance.GetLanguage().ToString() == "English")
            {
                english.SetActive(true);
                englishCredits.SetActive(true);
            }
            else if(SettingsManager.instance.GetLanguage().ToString() == "Danish")
            {
                danish.SetActive(true);
                danishCredits.SetActive(true);
            }
        }
    }

    private void SetupSoundSliders()
    {
        var soundManager = SoundManager.instance;

        masterSlider.value =  SettingsManager.instance.GetMasterVolume();
        musicSlider.value = SettingsManager.instance.GetMusicVolume();
        effectsSlider.value = SettingsManager.instance.GetEffectsVolume();
        
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

    public void OnSensitivitySliderChanged(float value)
    {
        SettingsManager.instance.SetSensitivity(value);
    }

    public void OnMuteToggle()
    {
        SettingsManager.instance.MuteSound(mute.isOn);
    }

    public void OnEnglishClick()
    {
        SettingsManager.instance.SetLanguage(Language.Danish);
        danish.SetActive(true);
        english.SetActive(false);
    }

    public void OnDanishClick()
    {
        SettingsManager.instance.SetLanguage(Language.English);
        danish.SetActive(false);
        english.SetActive(true);
    }

    public void OnCameraInvertedToggle()
    {
        SettingsManager.instance.SetInvertedCamera(cameraControls.isOn);

        var evt = new ObserverEvent(EventName.ToggleCameraControls);
        Subject.instance.Notify(gameObject, evt);
    }
}
