namespace LabTestBrowser.Infrastructure.Data.Settings;

public class HealthcareFacilityTrademark
{
	public required string Title { get; init; }
	public required IEnumerable<LabReportTemplate> ReportTemplates { get; init; }
}