using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExampleApi.Tests;

public class WeatherForecastControllerTests
{
	private readonly WebApplicationFactory<Program> webApplicationFactory;

	public WeatherForecastControllerTests()
	{
		webApplicationFactory = new WebApplicationFactory<Program>();
	}

	[Fact]
	public async Task ShouldReturnOkWhenHttpGetRequestIsReceived()
	{
		var httpClient = webApplicationFactory.CreateClient();

		var response = await httpClient.GetAsync("WeatherForecast");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task ShouldReturnValidForecastsOnGetRequest()
	{
		var httpClient = webApplicationFactory.CreateClient();

		var response = await httpClient.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast");

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
		var httpClient = webApplicationFactory.CreateClient();

		var response = await httpClient.GetAsync("WeatherForecast");

		response.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Json);
	}
}