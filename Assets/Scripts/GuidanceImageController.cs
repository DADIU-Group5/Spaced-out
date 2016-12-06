using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuidanceImageController : MonoBehaviour
{

    public Button button;
    public Sprite tutorialImage1;
    public Sprite tutorialImage2;
    private bool firstClick = true;

    // Use this for initialization
    void Start()
    {
        //button.gameObject.SetActive(false);
    }

    public void EnableImage()
    {
        enabled = true;
    }

    public void ChangeBackground()
    {
        if (firstClick)
        {
            button.image.sprite = tutorialImage2;
            firstClick = false;
        }
        else
            button.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        button.gameObject.SetActive(false);
        //ChangeBackground();
    }

    public void Activate()
    {
        button.gameObject.SetActive(true);
    }
}
