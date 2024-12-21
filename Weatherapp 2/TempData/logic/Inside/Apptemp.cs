using TempData.Models;

namespace TempData.Logic.Inside
{
    public class AppTemp
    {
        
        private readonly TempFuktDataContext _databas;
        public AppTemp(TempFuktDataContext databas)
        {
            _databas = databas;
        }


        public double AverageTempInHouse(DateTime date)
        {
            var data = _databas.TempFuktData
                .Where(row => row.Datum.Date == date.Date && row.Plats == "Inne")
                .Select(row => row.Temp)
                .ToList();

            if (data.Count == 0)
            {
                throw new InvalidOperationException("Zero inhouse found.");
            }
            return (double)data.Average();
        }

        public List<(DateTime Date, decimal AverageTemp)> SortHotToCold()
        {
            var result = _databas.TempFuktData
                .Where(row => row.Plats == "Inne")
                .GroupBy(row => row.Datum.Date)
                .Select(group => new
                {
                    Datum = group.Key,
                    AverageTemp = group.Average(row => row.Temp)
                })
                .OrderByDescending(item => item.AverageTemp)
                .ToList();

            return result
            .Select(item => (item.Datum, item.AverageTemp))
            .ToList();

        }

        public List<(DateTime Date, double AverageHumidTemp)> SortDryToHumid()
        {
            var result = _databas.TempFuktData
                .Where(row => row.Plats == "Inne")
                .GroupBy(row => row.Datum.Date)
                .Select(group => new
                {
                    Datum = group.Key,
                    AverageHumidTemp = group.Average(row => row.Luftfuktighet)
                })
                .OrderBy(item => item.AverageHumidTemp)
                .ToList();

            return result
                .Select(item => (item.Datum, (double)item.AverageHumidTemp))
                .ToList();
        }

        public List<(DateTime Date, decimal RiskForMold)> SortRiskForMold()
        {
            var result = _databas.TempFuktData
                .Where(row => row.Plats == "Inne")
                .GroupBy(row => row.Datum.Date)
                .Select(group => new
                {
                    Datum = group.Key,
                    RiskForMold = group.Average(row => (decimal)(row.Temp + row.Luftfuktighet) / 2)
                }) 
                .OrderBy(item => item.RiskForMold)
                .ToList();
            return result
                .Select(item => (item.Datum, item.RiskForMold))
                .ToList();

        }

    }
}