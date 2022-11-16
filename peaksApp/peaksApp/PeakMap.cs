using System;
using CsvHelper.Configuration;

namespace peaksApp
{
    public sealed class PeakMap : ClassMap<Peak>
    {
        public PeakMap()
        {
            Map(m => m.Name).Name(nameof(Peak.Name));
            Map(m => m.MountainRange).Name(nameof(Peak.MountainRange));
            Map(m => m.Elevation).Name(nameof(Peak.Elevation));
            Map(m => m.Season).Name(nameof(Peak.Season));
            Map(m => m.CrownOfPolishMountains).Name(nameof(Peak.CrownOfPolishMountains));
            Map(m => m.Country).Name(nameof(Peak.Country));
            Map(m => m.ExpeditionDate).Name(nameof(Peak.ExpeditionDate));
            Map(m => m.isConquered).Name(nameof(Peak.isConquered));
            Map(m => m.Latitude).Name(nameof(Peak.Latitude));
            Map(m => m.Longitude).Name(nameof(Peak.Longitude));
        }
    }
}