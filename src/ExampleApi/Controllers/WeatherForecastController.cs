using ExampleApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
	private static readonly string[] Summaries =
	{
		"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
	};

	private readonly ILogger<WeatherForecastController> _logger;
	private readonly WeatherDbContext context;

	public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherDbContext context)
	{
		_logger = logger;
		this.context = context;
	}

	[HttpGet(Name = "GetWeatherForecast")]
	public IEnumerable<WeatherForecast> Get()
	{
		return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			})
			.ToArray();
	}

	[HttpGet("{id:int}", Name = "GetWeatherForecastById")]
	public async Task<IActionResult> GetAsync(int id)
	{
		var weatherForecast = await context.WeatherForecasts.FindAsync(id);
		return Ok(weatherForecast);
	}
}