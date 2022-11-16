using System;
namespace peaksApp
{
    public class Peak
    {
        public string Name { get; set; }
        public MountainRange MountainRange { get; set; }
        public decimal Elevation { get; set; }
        public Season Season { get; set; }
        public bool CrownOfPolishMountains { get; set; }
        public string Country { get; set; }
        public DateTime ExpeditionDate { get; set; }
        public bool isConquered { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public override string ToString()
        {
            return $"{(Name.Length > 20 ? Name.Substring(0, 20) + "..." : Name),-23} | " +
                $"{MountainRange,-20} | " +
                $"{Elevation + "m n.p.m.",-15} | " +
                $"{Season,-10} | " +
                $"{CrownOfPolishMountains,-10} | " +
                $"{(Country.Length > 15 ? Country.Substring(0, 15) + "..." : Country), -18} | " +
                $"{ExpeditionDate.ToShortDateString() + " r.",-20} | " +
                $"{isConquered,-10} | " +
                $"{Latitude,-20} | " +
                $"{Longitude,-20} | ";
        }
    }
}
