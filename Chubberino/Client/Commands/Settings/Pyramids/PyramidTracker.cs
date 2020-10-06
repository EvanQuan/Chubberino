using System;
using System.Collections.Generic;

namespace Chubberino.Client.Commands.Settings.Pyramids
{
    public sealed class PyramidTracker
    {
        public HashSet<String> ContributorDisplayNames { get; }

        public String Block { get; set; }

        public Int32 CurrentHeight { get; set; }

        public Int32 TallestHeight { get; set; }

        public Boolean BuildingUp { get; set; }

        public PyramidTracker()
        {
            ContributorDisplayNames = new HashSet<String>();
            Reset();
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
}
