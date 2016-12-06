using System;
using System.Collections.Generic;

[Serializable]
public class Subtitles
{
    public SubtitlesByCategory englishSubtitles;
    public SubtitlesByCategory danishSubtitles;

    public List<Subtitle> GetSubtitles(Language language, SubtitleType type)
    {
        if (language == Language.English)
            return englishSubtitles.GetSubtitles(type);
        else
            return danishSubtitles.GetSubtitles(type);
    }

    public Subtitle GetSubtitle(string id, Language language)
    {
        var subtitles = language == Language.English ? englishSubtitles : danishSubtitles;

        // really not cool hack but no time :(
        var subList = id.Contains("narr") ? subtitles.narrative : subtitles.generalRemarks;

        return subList.Find(s => s.id == id);

        //if (language == Language.English)
        //    return englishSubtitles.narrative.Find(s => s.id == id);
        //else
        //    return danishSubtitles.narrative.Find(s => s.id == id);
    }
}
