using Ardalis.SharedKernel;
using MediatR;

namespace LabTestBrowser.UI.Configurations;

public static class MediatrConfigs
{
	public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
	{
		var mediatRAssemblies = AppDomain.CurrentDomain.GetAssemblies();

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies))
			.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
			.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

		return services;
	}
}