using TempData.Models;

namespace TempData.Logic.Outside
{
    internal class MeteorologicallAutumnStartCalculator
    {
        public void CalculateAutumnStart()
        {

            using (var context = new TempFuktDataContext())
            {
                bool autumnHappened = false;
                var result = context.TempFuktData
                   .Where(p => p.Plats == "Ute")                
                   .GroupBy(t => t.Datum.Date)                  
                   .Where(g => g.Max(t => t.Temp) <= 10)        
                   .OrderBy(g => g.Key)                         
                   .Select(g => g.Key)                          
                   .ToList();                                  

                for (var i = 0; i < result.Count - 4; i++)
                {
                    if ((result[i].AddDays(1).Day == result[i + 1].Day) &&                     
                        (result[i + 1].AddDays(1).Day == result[i + 2].Day) &&
                        (result[i + 2].AddDays(1).Day == result[i + 3].Day) &&
                        (result[i + 3].AddDays(1).Day == result[i + 4].Day))
                    {
                        autumnHappened = true;
                        Console.WriteLine("First day of the {0} meteorological Autumn: {1}", result[i + 5].Year, DateOnly.FromDateTime(result[i + 5]));
                        break;
                    }
                }
                if (autumnHappened == false)
                {
                    Console.WriteLine("{0} never had a meteorological autumn", result[0].Year);
                }
            }
        }
    }
}
