using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : Singleton<MainMenuManager> {

    private Button startBtn;
    private LevelSelectManager levelSelect;
    private Button settingsBtn;
    private Button shopBtn;
    private Button exitBtn;
    // The active view
    private GameObject view;

    public void OnStartBtnClick()
    {

    }

    public void OnLevelSelectBtnClick()
    {
        gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(true);
        view = levelSelect.gameObject;
    }

    public void OnSettingsBtnClick()
    {

    }

    public void OnShopBtnClick()
    {

    }

    public void Back()
    {
        view.SetActive(false);
    }
}
