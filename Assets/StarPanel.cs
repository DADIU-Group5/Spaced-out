using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarPanel : MonoBehaviour {

    public Text starText;

    void Start()
    {
        starText.text = ProgressManager.instance.GetTotalStars().ToString();
    }
}
