using CsvHelper.Configuration.Attributes;

namespace TempData.Models
{
    public class TempFuktData
    {
        [Ignore]
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Plats { get; set; }
        public decimal Temp { get; set; }
        public int Luftfuktighet { get; set; }
    }
}