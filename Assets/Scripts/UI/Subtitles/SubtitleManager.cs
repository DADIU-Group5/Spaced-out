using System;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleManager : Singleton<SubtitleManager>
{
    public TextAsset json;
    private Subtitles subtitles;

    // Use this for initialization
    void Start () {
        // Load of all subtitles
        subtitles = JsonUtility.FromJson<Subtitles>(json.text);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private Subtitle GetRandomSubtitle(List<Subtitle> subtitles)
    {
        if (subtitles.Count == 0)
            return new Subtitle();
        else
            return subtitles[UnityEngine.Random.Range(0, subtitles.Count)];
    }

    //private ObserverEvent MakeEventFromSubtitle(Language language, SubtitleType subType)
    //{
    //    var subtitleByCategory = subtitles.GetSubtitles(language, subType);
    //    var subtitle = GetRandomSubtitle(subtitleByCategory);

    //    var subtitleEvent = new ObserverEvent(EventName.ShowSubtile);

    //    subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_TEXT, subtitle.text);
    //    subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_START, subtitle.start);
    //    subtitleEvent.payload.Add(PayloadConstants.SUBTITLE_DURATION, subtitle.duration);

    //    return subtitleEvent;
    //}

    public Subtitle GetRandomSubtitle(Language language, SubtitleType subType)
    {
        var subtitleByCategory = subtitles.GetSubtitles(language, subType);

        return GetRandomSubtitle(subtitleByCategory);
    }

    public Subtitle GetSubtitle(string id, Language language)
    {
        return subtitles.GetSubtitle(id, language);
    }
}
