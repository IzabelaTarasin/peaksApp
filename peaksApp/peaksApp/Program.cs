using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace peaksApp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string csvPath = @"../../myPeaks.csv";
            var peaks = LoadPeaksFromCSV(csvPath);

            DisplayPeaks(peaks);
        }

        public static void DisplayPeaks(IEnumerable<Peak> peaks)
        {
            foreach (var peak in peaks)
            {
                Console.WriteLine(peak);
            }
        }

        public static List<Peak> LoadPeaksFromCSV(string csvPath)
        {
            Console.WriteLine("test 0" + csvPath);
            using(var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                Console.WriteLine("test" + reader);
                Console.WriteLine("test2" + csv);
                csv.Context.RegisterClassMap<PeakMap>();
                var records = csv.GetRecords<Peak>().ToList();
                return records;
            }
        }

        
    }
}
