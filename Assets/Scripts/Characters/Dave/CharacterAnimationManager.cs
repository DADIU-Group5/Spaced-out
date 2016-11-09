using UnityEngine;
using System.Collections;

public class CharacterAnimationManager : Singleton<CharacterAnimationManager>
{
    private GameObject playerChar;

    void Start()
    {
        playerChar = GameObject.FindGameObjectWithTag("Player");
    }

    public void AnimationToggle(bool toggle)
    {
        playerChar.GetComponent<Animator>().enabled = toggle;
    }

    public void AnimationWallCollide()
    {

    }

    public void AnimationObjectCollide()
    {

    }
}
