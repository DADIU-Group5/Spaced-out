using System;
using System.Collections.Generic;

[Serializable]
public class SubtitlesByCategory
{ 
    public List<Subtitle> narrative;
    public List<Subtitle> generalRemarks;
    public List<Subtitle> wires;
    public List<Subtitle> switches;
    public List<Subtitle> fan;
    public List<Subtitle> gasLeak;
    public List<Subtitle> lowOxygen;
    public List<Subtitle> outOfOxygen;

    public List<Subtitle> GetSubtitles(SubtitleType type)
    {
        switch (type)
        {
            case SubtitleType.Narrative:
                return narrative;
            case SubtitleType.GeneralRemarks:
                return generalRemarks;
            case SubtitleType.Wires:
                return wires;
            case SubtitleType.SwitchPressed:
                return switches;
            case SubtitleType.Fan:
                return fan;
            case SubtitleType.GasLeak:
                return gasLeak;
            case SubtitleType.LowOxygen:
                return lowOxygen;
            case SubtitleType.OutOfOxygen:
                return outOfOxygen;
            default:
                return null;
        }
    }
}
