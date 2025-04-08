using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Data.Queries;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.Infrastructure.Hl7.Messaging.v231;
using LabTestBrowser.UseCases.Contributors.List;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReportTemplates;

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
			.AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
			.AddScoped<IDeleteContributorService, DeleteContributorService>()
			.AddScoped<ILabTestReportQueryService, LabTestReportQueryService>()
			.AddSingleton<ILabTestReportTemplateQueryService, LabTestReportTemplateQueryService>();

		
		var labReportSection = config.GetSection(nameof(LabReportSettings));
		var labReportSettings = labReportSection.Get<LabReportSettings>();
		Guard.Against.Null(labReportSettings);
		services.AddSingleton<ILabTestReportTemplateQueryService>(LabTestReportTemplateQueryService.Create(labReportSettings));
		
		logger.LogInformation("{Project} services registered", "Infrastructure");

		return services;
	}
}