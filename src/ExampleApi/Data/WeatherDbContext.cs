using ExampleApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleApi.Data;

public class WeatherDbContext : DbContext
{
	public DbSet<WeatherForecastEntity> WeatherForecasts { get; set; }

	public WeatherDbContext(DbContextOptions options) : base(options)
	{
	}
}