using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDController : MonoBehaviour, Observer {

    public Text chargeText;
    public Text launchText;
    public Text statusText;
    public Text subtitleText;
    public Text camControlsText;
    public Text velocityText;
    public Text currentFuelText;
    public Text comicsLeftText;
    public Text shotText;

    public GameObject winMenu;

    public GameObject subtitleBackdrop;

    public Transform chargeArrow;
    public RectTransform chargeImagePivot,
        chargeMaskPivot;

    private float chargeArrowYMin = 68f;
    private float chargeArrowYHeight = 350.0f;

    private bool gameOver = false;

    void Awake ()
    {
        Subject.instance.AddObserver(this);
        //animator.StartPlayback();
        //gal.enabled = false;
    }

    void Start()
    {
        SettingsManager.instance.onLanguageChanged += UpdateButtonText;
        UpdateButtonText(SettingsManager.instance.GetLanguage());
    }

    private void UpdateButtonText(Language lan)
    {
        //camControlsText.text = Translator.instance.Get("invert camera controls");
        velocityText.text = Translator.instance.Get("velocity");
        //currentFuelText.text = Translator.instance.Get("current") + " " + Translator.instance.Get("fuel");
    }

    public void ToggleCameraControls()
    {
        var evt = new ObserverEvent(EventName.ToggleCameraControls);
        Subject.instance.Notify(gameObject, evt);
    }

    public void ShotsFired()
    {
        shotText.text = (""+ScoreManager.shotsFired).Replace("0", "O");
    }


    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            //case EventName.UpdateOxygen:
            //    var fuelPayload = evt.payload;
            //    int fuel = (int)fuelPayload[PayloadConstants.OXYGEN];
            //    fuelText.text = Translator.instance.Get("current") + " " + Translator.instance.Get("fuel") + ": " + fuel.ToString();

            //    break;

            case EventName.LaunchPowerChanged:
                var launchPayload = evt.payload;
                Vector2 launch = (Vector2)launchPayload[PayloadConstants.LAUNCH_FORCE];

                float t = launch.x / launch.y;

                chargeText.text = launch.x.ToString();
                chargeArrow.position = new Vector3(chargeArrow.position.x, chargeArrowYMin + chargeArrowYHeight * launch.x / launch.y);

                chargeMaskPivot.rotation = Quaternion.Euler(0f, 0f, (1 - t) * 180);

                chargeImagePivot.rotation = Quaternion.Euler(0f, 0f, 180f);

                break;

            case EventName.UpdateVelocity:
                var velocityPayload = evt.payload;
                string velocity = "";
                velocity = (string)velocityPayload[PayloadConstants.VELOCITY];
                string[] substrings = velocity.Split('/');

                launchText.text = Translator.instance.Get("velocity") + ": " 
                    + substrings[0] + "\n" + Translator.instance.Get(substrings[1]); ;

                break;

            case EventName.UpdateStatus:
                var statusPayload = evt.payload;
                string status = (string)statusPayload[PayloadConstants.STATUS];

                statusText.text = Translator.instance.Get(status);
                break;
            case EventName.PlayerDead:
                gameOver = true;
                break;
            case EventName.PlayerWon:
                gameOver = true;
                break;

            case EventName.Narrate:
                string subText = (string)evt.payload[PayloadConstants.SUBTITLE_TEXT];
                float subStart = (float)evt.payload[PayloadConstants.SUBTITLE_START];
                float subDuration = (float)evt.payload[PayloadConstants.SUBTITLE_DURATION];

                StartCoroutine(ShowSubtitle(subText, subStart, subDuration));
                break;
            case EventName.ComicsUpdate:
                var comicsPayload = evt.payload;
                if (comicsLeftText != null)
                {
                    comicsLeftText.text = ((string)comicsPayload[PayloadConstants.COMICS]).Replace("0", "O");
                    string tempText = ((string)comicsPayload[PayloadConstants.COMICS]).Replace("0", "O");
                    if (winMenu != null)
                    {
                        winMenu.GetComponent<WinMenu>().SetComics(tempText);
                    }
                }
                break;

            case EventName.ToggleUI:
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            case EventName.PlayerLaunch:
                ShotsFired();
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Subject.instance.RemoveObserver(this);
    }

    /// <summary>
    /// Handle displaying the subtitle to the screen
    /// </summary>
    public IEnumerator ShowSubtitle(string subText, float subStart, float subDuration)//, int emotion)
    {
        yield return new WaitForSeconds(subStart);

        subtitleText.text = subText;
        subtitleBackdrop.SetActive(true);

        var evt = new ObserverEvent(EventName.GALAnimate);
        evt.payload.Add(PayloadConstants.START_STOP, true);
        Subject.instance.Notify(gameObject, evt);

        yield return new WaitForSeconds(subDuration);

        subtitleText.text = "";
        subtitleBackdrop.SetActive(false);
    }
}
