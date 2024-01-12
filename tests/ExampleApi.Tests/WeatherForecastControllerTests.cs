using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExampleApi.Tests;

public class WeatherForecastControllerTests
{
	[Fact]
	public async Task ShouldReturnOkWhenHttpGetRequestIsReceived()
	{
		var httpClient = new WebApplicationFactory<Program>().CreateClient();

		var response = await httpClient.GetAsync("WeatherForecast");

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}
}