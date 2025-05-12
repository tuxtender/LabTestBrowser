using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;
using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

namespace LabTestBrowser.Desktop.Configurations;

public static class UseCaseConfigs
{
	public static IServiceCollection AddUseCaseConfigs(this IServiceCollection services)
	{
		services.AddTransient<IGetUpdatedCompleteBloodCountsUseCase, GetUpdatedCompleteBloodCountsUseCase>();
		services.AddTransient<IProcessHl7RequestUseCase, ProcessHl7RequestUseCase>();

		return services;
	}
}