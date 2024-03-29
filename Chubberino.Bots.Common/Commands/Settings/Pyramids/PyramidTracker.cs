﻿namespace Chubberino.Bots.Common.Commands.Settings.Pyramids;

public sealed class PyramidTracker
{
    public System.Collections.Generic.HashSet<String> ContributorDisplayNames { get; }

    public String Block { get; private set; }

    public Int32 CurrentHeight { get; private set; }

    public Int32 TallestHeight { get; private set; }

    public Boolean BuildingUp { get; private set; }

    /// <summary>
    /// Indicates that a pyramid has already started.
    /// </summary>
    public Boolean HasStarted => Block != null;

    public PyramidTracker()
    {
        ContributorDisplayNames = new();
        Reset();
    }

    public void Start(String displayName, String block)
    {
        Reset();
        Block = block;
        BuildUp(displayName);
    }

    public void BuildUp(String displayName)
    {
        BuildingUp = true;
        CurrentHeight++;
        TallestHeight++;
        ContributorDisplayNames.Add(displayName);
    }

    public void BuildDown(String displayName)
    {
        BuildingUp = false;
        CurrentHeight--;
        ContributorDisplayNames.Add(displayName);
    }

    public void Reset()
    {
        BuildingUp = true;
        CurrentHeight = 0;
        TallestHeight = 0;
        Block = null;
        ContributorDisplayNames.Clear();
    }
}
