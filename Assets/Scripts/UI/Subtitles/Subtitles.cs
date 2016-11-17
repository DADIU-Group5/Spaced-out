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
}
