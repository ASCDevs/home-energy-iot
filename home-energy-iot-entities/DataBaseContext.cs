using home_energy_iot_entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace home_energy_iot_entities
{
    public class DataBaseContext : DbContext
    {
        private IConfiguration _configuration;

        public DataBaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceReport> DevicesReports { get; set; }
        public DbSet<HouseBill> HousesBills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionString"]);
        }
    }
}