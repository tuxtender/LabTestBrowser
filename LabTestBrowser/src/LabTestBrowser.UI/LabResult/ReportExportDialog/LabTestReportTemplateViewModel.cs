namespace LabTestBrowser.UI.LabResult.ReportExportDialog;

public class LabTestReportTemplateViewModel
{
	public required int Id { get; init; }
	public required string Title { get; init; }
	public required string Path { get; init; }
	public bool IsSelected { get; set; }
}