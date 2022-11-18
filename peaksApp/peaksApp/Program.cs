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
            //GetPeaksHigherThen2000AndAreConquered(peaks);
            //GetPeaksHigherThen2000AndIsNotConquered(peaks);
            //GetWinterPeaks(peaks);
            //GetMainPointsOfPeaks(peaks);
            //GetFirstXPeaksInYSeason(peaks, 3, Season.LATO);
            //GetPeakInYSeasonIsNotConquered(peaks, Season.LATO);
            GetSortedPeaks(peaks);
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
            //return dto type
            Console.WriteLine("DTO type: ");
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
                    Longitude = p.Longitude,
                });

            foreach (var winterPeak in winterPeaksDto)
            {
                Console.WriteLine($"Zimowe wejście na {winterPeak.Name} {winterPeak.Elevation} m n.p.m. dnia {winterPeak.ExpeditionDate.ToShortDateString()}");
            }

            // return anonymous type
            Console.WriteLine("Anonymous type: ");
            var winterPeaksAnonymousDto = peaks
                .Where(p => p.Season == Season.ZIMA)
                .Select(p => new
                {
                    Name = p.Name,
                    MountainRange = p.MountainRange,
                    Elevation = p.Elevation,
                    Season = p.Season,
                    CrownOfPolishMountains = p.CrownOfPolishMountains,
                    Country = p.Country,
                    ExpeditionDate = p.ExpeditionDate,
                    isConquered = p.isConquered,
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                });

            foreach (var winterPeak in winterPeaksAnonymousDto)
            {
                Console.WriteLine($"Zimowe wejście na {winterPeak.Name} {winterPeak.Elevation} m n.p.m. dnia {winterPeak.ExpeditionDate.ToShortDateString()}");
            }
        }

        static void GetMainPointsOfPeaks(IEnumerable<Peak> peaks)
        {
            //IEnumerable<List<string>> :
            var mainPointsList = peaks.Select(p => p.MainPoints);
            foreach (var mainPointsItem in mainPointsList)
            {
                Console.WriteLine($"Select: {string.Join(", ", mainPointsItem)}");
            }

            //IEnumerable<string> :
            var mainPoints = peaks.SelectMany(p => p.MainPoints);
            Console.WriteLine($"SelectMany: {string.Join(", ", mainPoints)}");
        }

        static void GetFirstXPeaksInYSeason(IEnumerable<Peak> peaks, int x, Season y)
        {
            var firstXPeaks = peaks
                .Where(p => p.Season == y)
                .OrderBy(p => p.ExpeditionDate)
                .Take(x);

            DisplayPeaks(firstXPeaks);
        }

        /*
        static void GetPeakInYSeasonTakeWhileTest(IEnumerable<Peak> peaks, Season y)
        {
            var peaksIsNotConquered = peaks
                .Where(p => p.Season == y);
            DisplayPeaks(peaksIsNotConquered);

            try
            {
                var cos = peaksIsNotConquered
                    .TakeWhile(p =>
                    {
                        Console.WriteLine($"{p.MountainRange} {MountainRange.TATRY_ZACHODNIE}");
                        return p.MountainRange == MountainRange.TATRY_ZACHODNIE || p.MountainRange == MountainRange.BIESZCZADY_ZACHODNIE;
                    }).ToList();

                Console.WriteLine("test");
                DisplayPeaks(cos);

            }
            catch (Exception ex)
            {
                Console.WriteLine("ex: " + ex);
            }

        }
        */

        static void GetSortedPeaks(IEnumerable<Peak> peaks)
        {
            //sorted by elevation and date
            //IOrderedEnumerable<Peak>
            var sortedAscPeaks = peaks.OrderBy(p => p.Elevation).Take(3);
            Console.WriteLine("\nASC sorted by elevation");
            DisplayPeaks(sortedAscPeaks);

            var sortedDescPeaks = peaks.OrderByDescending(p => p.Elevation).Take(3);
            Console.WriteLine("\nDESC sorted by elevation");
            DisplayPeaks(sortedDescPeaks);

            var sortedPeaks2 = peaks.OrderBy(p => p.Elevation).ThenBy(p => p.ExpeditionDate).Take(3);
            Console.WriteLine("\nASC sorted by elevation and date");
            DisplayPeaks(sortedPeaks2);

            var sortedPeaks3 = peaks.OrderByDescending(p => p.Elevation).ThenByDescending(p => p.ExpeditionDate).Take(3);
            Console.WriteLine("\nDESC sorted by elevation and date");
            DisplayPeaks(sortedPeaks3);

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
