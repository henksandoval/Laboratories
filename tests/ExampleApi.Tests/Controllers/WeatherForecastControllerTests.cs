using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using ExampleApi.Data.Entities;
using FluentAssertions;

namespace ExampleApi.Tests.Controllers;

public class WeatherForecastControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
	private const string? RequestUri = "WeatherForecast";
	private readonly HttpClient httpClient;

	public WeatherForecastControllerTests(CustomWebApplicationFactory<Program> factory)
	{
		httpClient = factory.CreateClient();
	}

	[Fact]
	public async Task ShouldReturnOkWhenHttpGetRequestIsReceived()
	{
		//Act
		var response = await httpClient.GetAsync(RequestUri);

		//Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task ShouldReturnValidForecastsOnGetRequest()
	{
		//Act
		var response = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>(RequestUri);

		//Assert
		var expectedDate = DateOnly.FromDateTime(DateTime.Now);
		response.Should().AllSatisfy(forecast =>
		{
			forecast.Date.Should().BeOnOrAfter(expectedDate);
			forecast.TemperatureC.Should().BeInRange(-20, 55);
			forecast.Summary.Should().NotBeNullOrWhiteSpace();
		});
	}

	[Fact]
	public async Task ShouldReturnValidJsonOnGetRequest()
	{
		//Act
		var response = await httpClient.GetAsync(RequestUri);

		//Assert
		response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
	}

	[Fact]
	public async Task ShouldReturnRecordById()
	{
		//Arrange
		var expected = new WeatherForecastEntity
		{
			Date = DateTime.Today.AddDays(1),
			Summary = "Cool",
			TemperatureC = 4
		};

		//Act
		var response = await httpClient.GetFromJsonAsync<WeatherForecastEntity>($"{RequestUri}/{expected.Id}");

		//Assert
		response.Should().BeEquivalentTo(expected);
	}
}