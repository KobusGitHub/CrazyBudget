using Microsoft.EntityFrameworkCore;

namespace CrazyBudget.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Add DbSets here, e.g.
        // public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
