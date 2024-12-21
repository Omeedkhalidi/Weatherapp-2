using TempData.Models;


namespace TempData.Logic.Outside
{
    public class AverageOutsideTemperatureCalc
    {
        public void AverageOutsideTemp()
        {
           

            Console.Write("\nEnter a year (2016): ");
            string yearInput = Console.ReadLine();
            int year;
            bool result = int.TryParse(yearInput, out year);

            Console.Write("\nEnter a Month (10/11): ");
            string monthInput = Console.ReadLine();
            int month;
            result = int.TryParse(monthInput, out month);

            Console.Write("\nEnter a day (xx): ");
            string dayInput = Console.ReadLine();
            int day;
            result = int.TryParse(dayInput, out day);

            var targetDate = new DateTime(year, month, day);




           

            using (var context = new TempFuktDataContext())
            {
                var averageOutsideTemp = context.TempFuktData
                    .Where(t => t.Plats == ("Ute") && (targetDate == t.Datum.Date))       
                    .GroupBy(h => h.Datum.Date)                                           
                    .Select(g => new
                    {
                        Day = g.Key,                                                      
                        AvgTemp = g.Average(h => h.Temp)                                 
                    })
                    .ToList();

                Console.WriteLine("\nAverage outside temperature on {0} was {1} celcius", DateOnly.FromDateTime(averageOutsideTemp[0].Day), averageOutsideTemp[0].AvgTemp);
            }
        }
    }
}