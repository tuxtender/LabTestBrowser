﻿using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Data.Queries;
using LabTestBrowser.UseCases.Contributors.List;
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

		logger.LogInformation("{Project} services registered", "Infrastructure");

		return services;
	}
}