using ExampleApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleApi.Tests.Controllers;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Testing");
		base.ConfigureWebHost(builder);
	}

	public async Task AddRecordToDatabaseAsync<TEntity>(TEntity entity) where TEntity : class
	{
		using var scope = Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
		var dbSet = dbContext.Set<TEntity>();
		await dbSet.AddAsync(entity);
		await dbContext.SaveChangesAsync();
	}
}