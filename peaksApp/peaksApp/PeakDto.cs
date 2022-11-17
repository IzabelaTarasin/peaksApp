using System;
namespace peaksApp
{
    public class PeakDto
    {
        public string Name { get; set; }
        //public MountainRange MountainRange { get; set; }
        public decimal Elevation { get; set; }
        //public Season Season { get; set; }
        public bool CrownOfPolishMountains { get; set; }
        public string Country { get; set; }
        public DateTime ExpeditionDate { get; set; }
        public bool isConquered { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
