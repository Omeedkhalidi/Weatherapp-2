using Microsoft.EntityFrameworkCore;

namespace TempData.Models
{
    public class TempFuktDataContext : DbContext
    {
        public DbSet<TempFuktData> TempFuktData { get; set; }


        private const string connectstring =
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TempFuktDataDb_Omeed;Integrated Security=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectstring);
        }
    }
}
