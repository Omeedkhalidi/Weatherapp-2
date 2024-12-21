using CsvHelper;
using System.Globalization;
using Spectre.Console;
using TempData.Logic.Inside;
using TempData.Logic.Outside;
using TempData.Models;


namespace TempData
{
    public class Program
    {
        static void Main(string[] args)
        {

            string filePath = @"C:\Users\loveh\source\repos\Weatherapp 2\Weatherapp 2\TempData\Model\ExportData.csv";

            EnsureDbExists();
            ImportCsvToDatabase(filePath);

            using var context = new TempFuktDataContext();
            var appTemp = new AppTemp(context);

            bool exit = false;
            while (!exit)
            {
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .PageSize(20)
                        .AddChoices(new[] {
            "Outside Average Temp",
            "Outside Humidity Sorter",
            "Outside Temperature Sorter",
            "Outside Mould Risk Sorter",
            "Meteorological Autumn Start",
            "Meteorological Winter Start",
            "Inside AverageTemp",
            "Inside Sorted by Varm To Cold",
            "Inside Sorted by Dry To Humid",
            "Inside Sorted by Risk For Mold",
            "Update Database from Csv",
            "Close Program",

                        }));

                
                switch (selection)
                {
                    case "Outside Average Temp":
                        {
                            AverageOutsideTemperatureCalc aT = new AverageOutsideTemperatureCalc();
                            aT.AverageOutsideTemp();
                            break;
                        }
                    case "Outside Humidity Sorter":
                        {
                            AverageOutsideHumiditySorter aHT = new AverageOutsideHumiditySorter();
                            aHT.SortOutsideHumidity();
                            break;
                        }
                    case "Outside Temperature Sorter":
                        {
                            AverageOutsideTemperatureSorter aOT = new AverageOutsideTemperatureSorter();
                            aOT.SortOutsideTemperature();
                            break;
                        }
                    case "Outside Mould Risk Sorter":
                        {
                            OutsideMouldRiskSorter oMRS = new OutsideMouldRiskSorter();
                            oMRS.SortOutsideMoldRisk();
                            break;
                        }
                    case "Meteorological Autumn Start":
                        {
                            MeteorologicallAutumnStartCalculator mASC = new MeteorologicallAutumnStartCalculator();
                            mASC.CalculateAutumnStart();
                            break;
                        }
                    case "Meteorological Winter Start":
                        {
                            MeteorologicalWinterStartCalculator mWSC = new MeteorologicalWinterStartCalculator();
                            mWSC.CalculateWinterStart();
                            break;
                        }
                    case "Inside AverageTemp":
                        {
                            Console.WriteLine("Enter the Date from 2016-10-01 to 2016-11-31 is available: ");
                            DateTime inputDate = DateTime.Parse(Console.ReadLine());

                            double averageTemp = appTemp.AverageTempInHouse(inputDate);
                            Console.WriteLine($"Averagetemp inhouse for {inputDate:yyyy-MM-dd} is {averageTemp:F2}°C");
                            break;
                        }
                    case "Inside Sorted by Varm To Cold":
                        {
                            var hotToCold = appTemp.SortHotToCold();
                            Console.WriteLine("\nAverage Temp Hot to Cold days:");


                            foreach (var (Date, AverageTemp) in hotToCold)
                            {
                                Console.WriteLine($"{Date:yyyy-MM-dd}: {AverageTemp:F2}°C");
                            }
                            break;
                        }
                    case "Inside Sorted by Dry To Humid":
                        {
                            var dryToHumid = appTemp.SortDryToHumid();
                            Console.WriteLine("\nAverage Temp Dry to humid days:");

                            foreach (var (Date, AverageTemp) in dryToHumid)
                            {
                                Console.WriteLine($"{Date:yyyy-MM-dd}: {AverageTemp:F2}% Humid");
                            }
                            break;
                        }
                    case "Inside Sorted by Risk For Mold":
                        {
                            Console.WriteLine("<20 Low Risk - 20-40 Medium Risk - 40> High Risk");
                            var riskForMold = appTemp.SortRiskForMold();
                            Console.WriteLine("\nDays is sorted by risk for mold:");

                            foreach (var (Date, RiskForMold) in riskForMold)
                            {
                                Console.WriteLine($"{Date:yyyy-MM-dd}: Risk factor {RiskForMold:F2}");
                            }
                            break;
                        }
                    case "Update Database from Csv":
                        {
                            updateDatabaseFromCsv(filePath);
                            break;
                        }
                    case "Close Program":
                        {
                            exit = true;
                            break;
                        }
                }
                if (!exit)
                {

                    Console.ReadKey();
                    Console.Clear();
                }
            }

        }
        public static void EnsureDbExists()
        {
            using (var context = new TempFuktDataContext())
            {
                context.Database.EnsureCreated();
            }
        }
        public static void ImportCsvToDatabase(string filePath)
        {
            using var context = new TempFuktDataContext();

            bool hasData = context.TempFuktData.Any();           


            if (!hasData)                                        
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                
                var records = csv.GetRecords<TempFuktData>().ToList();

                
                context.TempFuktData.AddRange(records);
                context.SaveChanges();
                Console.WriteLine("Data Import from Csv done.\n");
            }
            else                                            
            {
                Console.WriteLine("Database already has data!\n\n\n");
            }
        }
        public static void updateDatabaseFromCsv(string filePath)
        {
            using (var context = new TempFuktDataContext())
            {
                context.TempFuktData.RemoveRange(context.TempFuktData);
                context.SaveChanges();
                Console.WriteLine("Database Cleared from old data");


                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<TempFuktData>().ToList();

                context.TempFuktData.AddRange(records);
                context.SaveChanges();
                Console.WriteLine("New data imported\n");
            }
        }
    }
}