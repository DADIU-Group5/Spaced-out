using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public class MenuLevelSelect : MonoBehaviour {

    public HorizontalScrollSnap snap;
    public LevelButtonAnimation[] buttonAnimations;
    public LevelButton[] buttons;
    private int index = 1;

    void Start()
    {
        buttonAnimations[1].SetFocus(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwipeRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwipeLeft();
        }
    }

    public void SwipeRight()
    {
        if (index < buttons.Length)
        {
            buttonAnimations[index].SetFocus(false);
            index++;
            buttonAnimations[index].SetFocus(true);
        }
    }

    public void SwipeLeft()
    {
        if (index > 0)
        {
            buttonAnimations[index].SetFocus(false);
            index--;
            buttonAnimations[index].SetFocus(true);
        }
    }

    public void LevelButtonsScrolled(Vector2 offset)
    {
        int focusedIndex = Mathf.RoundToInt(offset.x * 6);
        if (focusedIndex != index)
        {
            buttonAnimations[index].SetFocus(false);
            buttonAnimations[focusedIndex].SetFocus(true);
        }
    }


    public void UnlockAll()
    {
        ProgressManager.instance.UnlockAll();
    }

    // generates new seeds for levels
    public void GenerateNewSeeds()
    {
        // can we afford it?
        if (ProgressManager.instance.GetStars() > 15)
        {
            GenerationDataManager.instance.RandomizeSeeds();
            ProgressManager.instance.ChangeStars(-15);
        }
    }

    // loads the level
    public void LoadLevel(int level)
    {
        if (level == 0)
        {
            SceneManager.LoadScene("Intro Cinematic");
            GenerationDataManager.instance.SetTutortialLevel();
        }
        else
        {
            GenerationDataManager.instance.SetCurrentLevel(level);
            SceneManager.LoadScene("LevelGenerator");
        }
    }
}
