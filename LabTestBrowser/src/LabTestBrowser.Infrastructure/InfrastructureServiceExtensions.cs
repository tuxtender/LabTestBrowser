using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Data.Queries;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Mllp;
using LabTestBrowser.UseCases.Contributors.List;
using LabTestBrowser.UseCases.Export;
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

		var exportSettingsSection = config.GetSection(ExportOptions.SectionName);
		var exportSettings = exportSettingsSection.Get<ExportOptions>();
		Guard.Against.Null(exportSettings);
		services.Configure<ExportOptions>(exportSettingsSection);
		services.AddScoped<IExportService, ExportService>();
		services.AddSingleton<IExportFileNamingService, ExportFileNamingService>();
		services.AddSingleton<IFileTemplateEngine, ExcelTemplateEngine>();
		services.AddSingleton<ITextTemplateEngine, TextTemplateEngine>();
		services.AddSingleton<IExcelTemplateEngine, ExcelTemplateEngine>();
		services.AddSingleton<IWordTemplateEngine, WordTemplateEngine>();
		services.AddSingleton<ITemplateEngineResolver, TemplateEngineResolver>();

		var mllpSettingsSection = config.GetSection(MllpOptions.SectionName);
		var mllpSettings = mllpSettingsSection.Get<MllpOptions>();
		Guard.Against.Null(mllpSettings);
		services.Configure<MllpOptions>(mllpSettingsSection);
		services.AddSingleton<IMllpServerFactory, MllpServerFactory>();
		services.AddHostedService<MllpHostedService>();

		logger.LogInformation("{Project} services registered", "Infrastructure");

		return services;
	}
}