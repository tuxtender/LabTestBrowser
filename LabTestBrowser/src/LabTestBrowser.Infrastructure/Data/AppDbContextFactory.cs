using Microsoft.EntityFrameworkCore.Design;

namespace LabTestBrowser.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

		var services = new ServiceCollection();

		string? connectionString = config.GetConnectionString("SqliteConnection");
		Guard.Against.Null(connectionString);
		services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
		services.AddSingleton<IDomainEventDispatcher, NoOpDomainEventDispatcher>();

		var serviceProvider = services.BuildServiceProvider();
		return serviceProvider.GetRequiredService<AppDbContext>();
	}
}