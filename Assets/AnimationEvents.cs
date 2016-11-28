using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {

    // called once Dave's death animation is over 
    public void DeathAnimationOver()
    {
        GetComponentInParent<PlayerBehaviour>().DeathAnimationOver();
    }

    // called once Dave scratches his butt in idle animation
    public void ButtScratch()
    {

    }

    // called once Dave touches his helmet in idle animation
    public void HeadTouch()
    {

    }
}
