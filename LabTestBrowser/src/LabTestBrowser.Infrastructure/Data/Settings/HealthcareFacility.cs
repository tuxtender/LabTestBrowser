namespace LabTestBrowser.Infrastructure.Data.Settings;

public class HealthcareFacility
{
	public required string Supervisor { get; init; }
	public required IEnumerable<HealthcareFacilityTrademark> Trademarks { get; init; }
}