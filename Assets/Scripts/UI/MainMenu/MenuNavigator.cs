using UnityEngine;
using System.Collections;

public class MenuNavigator : MonoBehaviour {
    public GameObject mainView;
    private GameObject view;

    void Start()
    {
        view = mainView;
    }

	public void SetView(GameObject newView)
    {
        view.SetActive(false);
        if (view.GetComponent<MenuMain>() != null)
            view.GetComponent<MenuMain>().UpdateButtonText(SettingsManager.instance.GetLanguage());
        newView.SetActive(true);
        view = newView;
    }

    public void Back()
    {
        SetView(mainView);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
