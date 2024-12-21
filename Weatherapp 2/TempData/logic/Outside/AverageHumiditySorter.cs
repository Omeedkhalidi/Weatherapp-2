using TempData.Models;

namespace TempData.Logic.Outside
{
    public class AverageOutsideHumiditySorter
    {
        public void SortOutsideHumidity()
        {
            using (var context = new TempFuktDataContext())
            {
                var sortedOutsideHumidity = context.TempFuktData
                    .Where(h => h.Plats == ("Ute"))             
                    .GroupBy(h => h.Datum.Date)                 
                    .Select(g => new
                    {
                        Day = g.Key,                           
                        AvgHumidity = g.Average(h => h.Luftfuktighet)     
                    })

                    .OrderBy(result => result.AvgHumidity)      
                    .ToList();
               
                

                foreach (var s in sortedOutsideHumidity)
                {
                    Console.WriteLine("{0} Average outside humidity {1:F2}", s.Day.ToShortDateString(), s.AvgHumidity);
                };

            }
        }
    }
}
