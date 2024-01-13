using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExampleApi.Tests.Controllers;

public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
	private const string? RequestUri = "WeatherForecast";
	private readonly HttpClient httpClient;

	public WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
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
}