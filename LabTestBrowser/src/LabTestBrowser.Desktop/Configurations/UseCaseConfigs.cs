using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;

namespace LabTestBrowser.Desktop.Configurations;

public static class UseCaseConfigs
{
	public static IServiceCollection AddUseCaseConfigs(this IServiceCollection services)
	{
		services.AddTransient<IGetUpdatedCompleteBloodCountsUseCase, GetUpdatedCompleteBloodCountsUseCase>();

		return services;
	}
}