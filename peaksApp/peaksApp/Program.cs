using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace peaksApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string csvPath = @"../../myPeaks.csv";
            var peaks = LoadPeaksFromCSV(csvPath);

            //DisplayPeaks(peaks);
            GetPeaksHigherThen2000AndAreConquered(peaks);
            GetPeaksHigherThen2000AndIsNotConquered(peaks);
            GetWinterPeaks(peaks);
        }

        static void GetPeaksHigherThen2000AndAreConquered(IEnumerable<Peak> peaks)
        {
            var highestPeaks = peaks
                .Where(p => p.Elevation > 2000 && p.isConquered == true)
                .GroupBy(p => p.Name)
                .Select(p => p.FirstOrDefault());

            DisplayPeaks(highestPeaks);
        }

        static void GetPeaksHigherThen2000AndIsNotConquered(IEnumerable<Peak> peaks)
        {
            var highestPeaks = peaks
                .Where(p => p.Elevation > 2000 && p.isConquered == false)
                .GroupBy(p => p.Name)
                .Select(p => p.FirstOrDefault());

            DisplayPeaks(highestPeaks);
        }

        static void GetWinterPeaks(IEnumerable<Peak> peaks)
        {
            var winterPeaksDto = peaks
                .Where(p => p.Season == Season.ZIMA)
                .Select(p => new PeakDto() {
                    Name = p.Name,
                    MountainRange = p.MountainRange,
                    Elevation = p.Elevation,
                    Season = p.Season,
                    CrownOfPolishMountains = p.CrownOfPolishMountains,
                    Country = p.Country,
                    ExpeditionDate = p.ExpeditionDate,
                    isConquered = p.isConquered,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                });

            foreach (var winterPeak in winterPeaksDto)
            {
                Console.WriteLine($"Zimowe wejście na {winterPeak.Name} {winterPeak.Elevation} m n.p.m. dnia {winterPeak.ExpeditionDate.ToShortDateString()}");
            }
        }

        public static void DisplayPeaks(IEnumerable<Peak> peaks)
        {
            foreach (var peak in peaks)
            {
                Console.WriteLine(peak);
            }
        }

        public static void DisplayPeak(Peak peak)
        {
            Console.WriteLine(peak);
        }

        public static List<Peak> LoadPeaksFromCSV(string csvPath)
        {
            using(var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<PeakMap>();
                var records = csv.GetRecords<Peak>().ToList();
                return records;
            }
        }

        
    }
}
