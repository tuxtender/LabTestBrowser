using LabTestBrowser.Infrastructure.Data;

namespace LabTestBrowser.Desktop;

public static class DatabaseInitializerExtensions
{
	public static void EnsureDatabaseCreated(this IHost app)
	{
		using var scope = app.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		context.Database.EnsureCreated();
	}
}