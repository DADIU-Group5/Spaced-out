using UnityEngine;
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
    }

    private void UpdateButtonText(Language lan)
    {
        //medaltext.text = Translator.instance.Get("medals") + ":";//medaltext.text.ToString());
        ////hardcode
        //resetMedalstext.text = Translator.instance.Get("reset medals");
        //resetAlltext.text = Translator.instance.Get("reset all");
    }

    public void UpdateMedalLabel()
    {
        //medalLabel.text = ProgressManager.instance.GetStars().ToString();
    }

    public void ResetMedals()
    {
        ProgressManager.instance.ResetStars();
        medalLabel.text = "0";
    }

    public void ResetAll()
    {
        ProgressManager.instance.Reset();
        GenerationDataManager.instance.Reset();
        medalLabel.text = "0";
    }
}
