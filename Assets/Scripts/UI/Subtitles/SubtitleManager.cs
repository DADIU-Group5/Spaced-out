using System;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleManager : Singleton<SubtitleManager>, Observer
{
    public TextAsset json;
    private Subtitles subtitles;

    // Use this for initialization
    void Start () {
        // Register ourselves to receive events
        Subject.instance.AddObserver(this);

        // Load of all subtitles
        subtitles = JsonUtility.FromJson<Subtitles>(json.text);

        //Subtitle sub = new Subtitle();
        //sub.id = 1;
        //sub.text = "Hej, jeg hedder Giorgos";
        //sub.start = 0.0f;
        //sub.duration = 2.0f;

        //SubtitlesByCategory categories = new SubtitlesByCategory();
        //categories.narrative = new System.Collections.Generic.List<Subtitle>();
        //categories.narrative.Add(sub);

        //Subtitles subs = new Subtitles();
        //subs.englishSubtitles = categories;
        //subs.danishSubtitles = categories;

        //string jsonn = JsonUtility.ToJson(subs);
        //Debug.Log(json);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnNotify(GameObject entity, ObserverEvent evt)
    {
        switch (evt.eventName)
        {
            case EventName.OnFire:
                Subject.instance.Notify(gameObject, MakeEventFromSubtitle(Language.English, SubtitleType.GasLeak));
                break;
            case EventName.Electrocuted:
                Subject.instance.Notify(gameObject, MakeEventFromSubtitle(Language.Danish, SubtitleType.Wires));
                break;
            case EventName.OxygenEmpty:
                Subject.instance.Notify(gameObject, MakeEventFromSubtitle(Language.English, SubtitleType.OutOfOxygen));
                break;
            default:
                break;
        }
    }

    private Subtitle GetRandomSubtitle(List<Subtitle> subtitles)
    {
        if (subtitles.Count == 0)
            return new Subtitle();
        else
            return subtitles[UnityEngine.Random.Range(0, subtitles.Count)];
    }

    private ObserverEvent MakeEventFromSubtitle(Language language, SubtitleType subType)
    {
        var subtitleByCategory = subtitles.GetSubtitles(language, subType);
        var subtitle = GetRandomSubtitle(subtitleByCategory);

        var subtitleEvent = new ObserverEvent(EventName.ShowSubtile);

        subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_TEXT, subtitle.text);
        subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_START, subtitle.start);
        subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_DURATION, subtitle.duration);

        return subtitleEvent;
    }

    public Subtitle GetRandomSubtitle(Language language, SubtitleType subType)
    {
        var subtitleByCategory = subtitles.GetSubtitles(language, subType);

        return GetRandomSubtitle(subtitleByCategory);
    }
}
