using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Data.Queries;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.Infrastructure.Mllp;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.Infrastructure;

public static class InfrastructureServiceExtensions
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
		ConfigurationManager config,
		ILogger logger)
	{
		string? connectionString = config.GetConnectionString("SqliteConnection");
		Guard.Against.Null(connectionString);
		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlite(connectionString));

		services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
			.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
			.AddScoped<ILabTestReportQueryService, LabTestReportQueryService>();

		services.AddSingleton<IMllpServerFactory, MllpServerFactory>();
		services.AddHostedService<MllpHostedService>();

		services.AddSingleton<IV231OruR01Converter, V231OruR01Converter>();
		services.AddSingleton<IHl7AcknowledgmentService, Hl7AcknowledgmentService>();

		services.AddSingleton<CompleteBloodCountUpdateChannel>();
		services.AddSingleton<ICompleteBloodCountUpdateNotifier>(provider => provider.GetRequiredService<CompleteBloodCountUpdateChannel>());
		services.AddSingleton<ICompleteBloodCountUpdateReader>(provider => provider.GetRequiredService<CompleteBloodCountUpdateChannel>());

		logger.LogInformation("{Project} services registered", "Infrastructure");

		return services;
	}
}