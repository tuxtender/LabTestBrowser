using LabTestBrowser.Infrastructure.Data;

namespace LabTestBrowser.UI;

public class RecoverDatabaseBackgroundService(AppDbContext _dbContext, ILogger<RecoverDatabaseBackgroundService> _logger)
	: BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var isDbCreated = await _dbContext.Database.EnsureCreatedAsync(stoppingToken);

		if (isDbCreated)
			_logger.LogInformation("Database created");

		_logger.LogInformation("Database found");
	}
}