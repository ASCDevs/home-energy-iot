using home_energy_api.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace home_energy_api.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Device> devices { get; set; }
        public DbSet<HouseUser> houses { get; set; }
        public DbSet<HouseBill> houseBills { get; set; }
        public DbSet<ReportDevice> reportDevices { get; set; }
        public DbSet<User> users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
