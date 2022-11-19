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
            //GetPeaksFromCrownOfPolishMountainsHigherThan1300(peaks);
            //GetPeaksFrom2021(peaks);
            //GetAllPeaksGroupedByMountainRange(peaks);
            //GetPeaksFromXMountainRange(peaks, MountainRange.TATRY_WYSOKIE);
            GetAllPeaksGroupedByMountainRange2(peaks);

        }

        static void GetAllPeaksGroupedByMountainRange(IEnumerable<Peak> peaks)
        {
            /*
            //return IEnumerable<IGrouping<Category,Peak>>
            var mountainRangeGroup = peaks.GroupBy(p => p.MountainRange);
            */

            //return List<IGrouping<MountainRange,Peak>>
            var mountainRangeGroup = peaks.GroupBy(p => p.MountainRange).ToList();

            foreach (var p in mountainRangeGroup)
            {
                Console.WriteLine($"\nGroup: {p.Key}");

                foreach (var pmr in p) {
                    Console.WriteLine($"Peak: {pmr.Name, -20} date: {pmr.ExpeditionDate.ToShortDateString()}");
                }
            }
        }

        static void GetAllPeaksGroupedByMountainRange2(IEnumerable<Peak> peaks)
        {
            //return List<IGrouping<MountainRange,Peak>>
            var mountainRangeGroup = peaks.OrderBy(p => p.Season).GroupBy(p => new { p.MountainRange, p.Season });

            foreach (var mrg in mountainRangeGroup)
            {
                //key is new anonymous object with MountainRange and Season properties
                var key = mrg.Key;
                
                Console.WriteLine($"Peak: {key.MountainRange,-20} Season: {key.Season}");
            }
        }

        static void GetPeaksFromXMountainRange(IEnumerable<Peak> peaks, MountainRange mountainRange)
        {
            var peaksGruoup = peaks
                .GroupBy(p => p.MountainRange)
                .First(p => p.Key == mountainRange);

            //2 sposoby wyswietlania: (lub od razu w powyzszej metodzie linq)
            var peaks1 = peaksGruoup.Select(p => p);
            DisplayPeaks(peaks1);
            var peaks2 = peaksGruoup.ToList();
            DisplayPeaks(peaks2);
        }

        static void GetPeaksFrom2021(IEnumerable<Peak> peaks)
        {
            //where filtrowanie danych na podstawie predykaty
            var peaks2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01));
            DisplayPeaks(peaks2021);

            //first - pierwszy element z kolekcji
            var onePeak2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).First();
            Console.WriteLine("\nFirst peak from 2021 collection");
            DisplayPeak(onePeak2021);

            //first - pierwszy element z kolekcji spęłniający dodatkowy warunek na podstawie wrowadzonego predykatu
            //gdy żaden element nie spełni warunku zostanie rzucony wyjątek
            var oneWinterPeak2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).First(p => p.Season == Season.ZIMA);
            Console.WriteLine("\nFirst peak from winter 2021 collection");
            DisplayPeak(oneWinterPeak2021);

            //firstOrDefault - zwroci pierwszy element z kolekcji a gdy zaden element nie spelni warunku to nie wyrzuci wyjatku tylko zapisze domyślą wartość dla danego typu
            var oneWinterPeak2021FD = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).FirstOrDefault(p => p.Season == Season.JESIEŃ);
            Console.WriteLine("\nFirst peak from winter 2021 collection - first or default");
            DisplayPeak(oneWinterPeak2021FD);

            /*
            //single - sprawdzi czy w elementach spelniajacych predykate znajduje sie wiecej niz 1 element (wyrzuci blad w trakcie dzialania programu gdy jest wiecej niz 1 element)
            var oneSummerPeak2021SingleMethod = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).Single(p => p.Season == Season.LATO);
            Console.WriteLine("\nFirst  peak from summer 2021 collection - single");
            DisplayPeak(oneSummerPeak2021SingleMethod); //throw System.InvalidOperationException: Sequence contains more than one matching element
            */

            /*
            var oneSpringPeak2021SingleMethod = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).Single(p => p.Season == Season.WIOSNA);
            Console.WriteLine("\nFirst peak from spring 2021 collection - single");
            DisplayPeak(oneSpringPeak2021SingleMethod); //throw System.InvalidOperationException: Sequence contains no matching element
            */

            var onePeak2021SingleMethod = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).Single(p => p.MountainRange == MountainRange.POGÓRZE_WIŚNICKIE);
            Console.WriteLine("\nFirst peak from winter 2021 collection - single");
            DisplayPeak(onePeak2021SingleMethod); //only one element

            //single or default - gdy nie spelni warunku to przypisze wartosc defaultową dla danego typu, nie wyrzuci wyjatku jak single
            var oneSpringPeak2021SingleMethod = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).SingleOrDefault(p => p.Season == Season.WIOSNA);
            Console.WriteLine("\nFirst peak from spring 2021 collection - single");
            DisplayPeak(oneSpringPeak2021SingleMethod);

            //last - wybierze ostatni element z kolekcji
            var lastPeak2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).Last();
            Console.WriteLine("\nLast peak from 2021 collection");
            DisplayPeak(lastPeak2021);

            //last - wybierze ostatni elemnt spelniajacy dany predykat, gdy nie zostanie znaleziony to rzuci wyjatkiem
            var lastSummerPeak2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).Last(p => p.Season == Season.LATO);
            Console.WriteLine("\nLast peak from summer 2021 collection");
            DisplayPeak(lastSummerPeak2021);

            //last or default - gdy na podstawie predykatu nie znajdzie zadnego elementu to przypisze wartosc domyslną dla danego typu
            var lastSpringPeak2021 = peaks.Where(p => p.ExpeditionDate >= new DateTime(2021, 01, 01)).LastOrDefault(p => p.Season == Season.WIOSNA);
            Console.WriteLine("\nLast peak from spring 2021 collection");
            DisplayPeak(lastSpringPeak2021);
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
