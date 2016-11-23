using System;

public static class ExtensionMethods
{
    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

    public static SubtitleType EventToSubtitleType(this EventName value)
    {
        return (SubtitleType)EventName.Parse(typeof(SubtitleType), value.ToString());
    }

    public static EventName SubtitleToEventType(this SubtitleType value)
    {
        return (EventName)SubtitleType.Parse(typeof(EventName), value.ToString());
    }
}
