using TempData.Models;

namespace TempData.Logic.Outside
{
    public class AverageOutsideTemperatureSorter
    {
        public void SortOutsideTemperature()
        {
            using (var context = new TempFuktDataContext())
            {
                var sortedOutsideHumidity = context.TempFuktData
                    .Where(h => h.Plats == ("Ute"))                      
                    .GroupBy(h => h.Datum.Date)                          
                    .Select(g => new
                    {
                        Day = g.Key,                                     
                        AvgTemperature = g.Average(h => h.Temp)          
                    })

                    .OrderByDescending(result => result.AvgTemperature)  
                    .ToList();
               
                

                foreach (var s in sortedOutsideHumidity)                
                {
                    Console.WriteLine("{0}. Average outside temperature {1:F2}", DateOnly.FromDateTime(s.Day), s.AvgTemperature);
                };

            }
        }
    }
}