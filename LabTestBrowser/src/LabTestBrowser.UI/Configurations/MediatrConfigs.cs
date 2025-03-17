using System.Reflection;
using Ardalis.SharedKernel;
using LabTestBrowser.Core.ContributorAggregate;
using LabTestBrowser.UseCases.Contributors.Create;
using MediatR;

namespace LabTestBrowser.UI.Configurations;

public static class MediatrConfigs
{
	public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
	{
		var mediatRAssemblies = new[]
		{
			Assembly.GetAssembly(typeof(Contributor)), // Core
			Assembly.GetAssembly(typeof(CreateContributorCommand)) // UseCases
		};

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
			.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
			.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

		return services;
	}
}