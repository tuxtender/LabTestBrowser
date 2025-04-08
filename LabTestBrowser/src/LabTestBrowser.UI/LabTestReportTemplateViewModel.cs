namespace LabTestBrowser.UI;

public class LabTestReportTemplateViewModel
{
	public required string Title { get; init; }
	public required string Path { get; init; }
	public bool IsSelected { get; set; }
}