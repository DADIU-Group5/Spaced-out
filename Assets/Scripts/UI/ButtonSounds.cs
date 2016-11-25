using UnityEngine;
using System.Collections;

public class ButtonSounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MenuClickForwards()
    {
        var evt = new ObserverEvent(EventName.UIButton);
        evt.payload.Add(PayloadConstants.TYPE, SoundEventConstants.MENU_CLICK_FORWARDS);
        Subject.instance.Notify(gameObject, evt);
    }

    public void MenuClickBackwards()
    {
        var evt = new ObserverEvent(EventName.UIButton);
        evt.payload.Add(PayloadConstants.TYPE, SoundEventConstants.MENU_CLICK_BACKWARDS);
        Subject.instance.Notify(gameObject, evt);
    }

    public void MenuPressStart()
    {
        var evt = new ObserverEvent(EventName.UIButton);
        evt.payload.Add(PayloadConstants.TYPE, SoundEventConstants.MENU_PRESS_START);
        Subject.instance.Notify(gameObject, evt);
    }

    public void MenuScroll()
    {
        var evt = new ObserverEvent(EventName.UIButton);
        evt.payload.Add(PayloadConstants.TYPE, SoundEventConstants.MENU_SCROLL);
        Subject.instance.Notify(gameObject, evt);
    }
}
