using LabTestBrowser.Infrastructure.Mllp;
using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

namespace LabTestBrowser.Desktop.Configurations;

public static class MllpServerConfigs
{
	public static IServiceCollection AddMllpServerConfigs(this IServiceCollection services)
	{
		services.AddSingleton<IMllpHost>(provider =>
		{
			var factory = provider.GetRequiredService<IMllpServerFactory>();
			var handler = provider.GetRequiredService<IProcessHl7RequestUseCase>();
			var mllpHost = factory.Create(async message => await handler.ExecuteAsync(message));
			return mllpHost;
		});

		return services;
	}
}