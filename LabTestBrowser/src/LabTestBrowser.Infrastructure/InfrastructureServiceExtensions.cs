using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Data.Queries;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.Infrastructure.Mllp;
using LabTestBrowser.UseCases.Contributors.List;
using LabTestBrowser.UseCases.Export;
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
			.AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
			.AddScoped<IDeleteContributorService, DeleteContributorService>()
			.AddScoped<ILabTestReportQueryService, LabTestReportQueryService>();

		
		var labReportSection = config.GetSection(nameof(LabReportSettings));
		var labReportSettings = labReportSection.Get<LabReportSettings>();
		Guard.Against.Null(labReportSettings);
		var section = config.GetSection(nameof(AnimalSettings));
		var animalSettings = section.Get<AnimalSettings>();
		Guard.Against.Null(animalSettings);
		var queryServiceFactory = new QueryServicesFromConfigFactory(labReportSettings, animalSettings);
		services.AddSingleton(queryServiceFactory.CreateLabTestReportTemplateQueryService());
		services.AddSingleton(queryServiceFactory.CreateListSpecimenCollectionCentersQueryService());
		services.AddSingleton(queryServiceFactory.CreateListAnimalSpeciesQueryService());

		services.AddScoped<IExportService, ExportService>();
		services.AddSingleton<IExportFileNamingService, ExportFileNamingService>();
		services.AddSingleton<IFileTemplateEngine, ExcelTemplateEngine>();
		services.AddSingleton<ITextTemplateEngine, TextTemplateEngine>();
		services.AddSingleton<IExcelTemplateEngine, ExcelTemplateEngine>();
		services.AddSingleton<IWordTemplateEngine, WordTemplateEngine>();
		services.AddSingleton<ITemplateEngineResolver, TemplateEngineResolver>();

		services.AddSingleton<IMllpServerFactory, MllpServerFactory>();
		services.AddHostedService<MllpHostedService>();

		services.AddSingleton<IV231OruR01Converter, V231OruR01Converter>();
		services.AddSingleton<IHl7AcknowledgmentService, Hl7AcknowledgmentService>();
		services.AddSingleton<ICompleteBloodCountUpdateChannel, CompleteBloodCountUpdateChannel>();

		logger.LogInformation("{Project} services registered", "Infrastructure");

		return services;
	}
}