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
            //GetSortedPeaks(peaks);
            //GetConqueredPeaks(peaks);
            //GetPeaksFromDifferentSets(peaks);
            GetPeaksFromCrownOfPolishMountainsHigherThan1300(peaks);
        }

        static void GetPeaksFromCrownOfPolishMountainsHigherThan1300(IEnumerable<Peak> peaks)
        {
            Console.WriteLine("crownPeaks:");
            var crownPeaks = peaks.Where(p => p.CrownOfPolishMountains == true);
            DisplayPeaks(crownPeaks);

            Console.WriteLine("\ncrownPeaksHigherThan1300:");
            var crownPeaksHigherThan1300 = peaks.Where(p => p.CrownOfPolishMountains == true && p.Elevation > 1300);
            DisplayPeaks(crownPeaksHigherThan1300);

            //all - czy elementy spelniaja warunek
            var allOperatorPeaks = peaks.Where(p => p.CrownOfPolishMountains == true).All(p => p.Elevation >1300);
            Console.WriteLine(allOperatorPeaks); //false bo przynajmniej jeden peak ma mniej niż 1300 lub = 1300

            var allOperatorPeaksT = peaks.Where(p => p.CrownOfPolishMountains == true).All(p => p.Elevation > 10);
            Console.WriteLine(allOperatorPeaksT); //true bo wszystkie emeenty spelniaja warunek

            //any - czy jest choc jeden element ktory spelnia warunek
            var anyOperatorPeaks = peaks.Where(p => p.CrownOfPolishMountains == true).Any(p => p.Elevation > 1300);
            Console.WriteLine(anyOperatorPeaks); //true bo przynajmniej jeden peak ma więcej niż 1300

            var anyOperatorPeaksN = peaks.Where(p => p.CrownOfPolishMountains == true).Any(p => p.Elevation > 2500);
            Console.WriteLine(anyOperatorPeaksN); //false bo zaden element nie spelnia warunku
        }

        static void GetConqueredPeaks(IEnumerable<Peak> peaks)
        {
            //Distinct operuje na 1 zbiorze
            //zdobyte wierzchołki wyswietlic tylko nazwy
            //return IEnumerable<string>
            var conqueredPeaksName = peaks.Where(p => p.isConquered == true).Select(p => p.Name).Distinct();
            Console.WriteLine($"{string.Join(",\n", conqueredPeaksName)}");
        }

        static void GetPeaksFromDifferentSets(IEnumerable<Peak> peaks)
        {
            var winterPeaks = peaks.Where(p => p.Season == Season.ZIMA);
            Console.WriteLine("Winter:");
            DisplayPeaks(winterPeaks);

            var tatrasPeaks = peaks.Where(p => p.MountainRange == MountainRange.TATRY_WYSOKIE);
            Console.WriteLine("\nTatry wysokie:");
            DisplayPeaks(tatrasPeaks);

            //Union - scalenie zbiorów bez duplikowania rekordów, łączenie tylko tych zbiorów ktorych typy się pokrywają
            var unionPeaks = winterPeaks.Union(tatrasPeaks);
            Console.WriteLine("\nUnion:");
            DisplayPeaks(unionPeaks);

            //Intersect - zwraca tylko te wartosci ktore wystepują w obu zbiorach, łączenie tylko tych zbiorów ktorych typy się pokrywają
            var intersectPeaks = winterPeaks.Intersect(tatrasPeaks);
            Console.WriteLine("\nIntersect:");
            DisplayPeaks(intersectPeaks);

            //except, wywolanie metody na zbiorze a, zwroci te rekordy ktore sa w zbiorze a i nie wystepuja w zbiorze b
            var exceptPeaks = winterPeaks.Except(tatrasPeaks);
            Console.WriteLine("\nExcept:");
            DisplayPeaks(exceptPeaks);

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
