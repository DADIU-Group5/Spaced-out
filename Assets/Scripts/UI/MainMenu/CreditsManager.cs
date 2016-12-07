using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{

    public GameObject danishCredits;
    public GameObject englishCredits;

    // Use this for initialization
    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            if (SettingsManager.instance.GetLanguage().ToString() == "English")
            {
                englishCredits.SetActive(true);
            }
            else if (SettingsManager.instance.GetLanguage().ToString() == "Danish")
            {
                danishCredits.SetActive(true);
            }
        }
    }
}
