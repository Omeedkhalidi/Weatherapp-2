using TempData.Models;

namespace TempData.Logic.Outside
{
    public class OutsideMouldRiskSorter
    {
        public void SortOutsideMoldRisk()
        {
            decimal constant = 0.35M;
            using (var context = new TempFuktDataContext())
            {
                var sortedOutsideHumidity = context.TempFuktData
                    .Where(h => h.Plats == ("Ute"))                             
                    .GroupBy(h => h.Datum.Date)                                 
                    .Select(g => new
                    {
                        Day = g.Key,                                            
                        MouldRisk = g.Average(x => (decimal)x.Luftfuktighet) - (constant * g.Average(x => x.Temp))  
                                                                                                                    
                                                                                                                    
                                                                                                                    
                                                                                                                    
                    })

                    .OrderBy(result => result.MouldRisk)                        
                    .ToList();

                foreach (var s in sortedOutsideHumidity)
                {
                    Console.WriteLine("{0}. Outside Mould Risk {1:F2}", s.Day.ToShortDateString(), s.MouldRisk);
                };

            }
        }
    }
}
