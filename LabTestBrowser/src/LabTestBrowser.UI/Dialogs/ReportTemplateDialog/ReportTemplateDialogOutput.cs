using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.UI.Dialogs.ReportTemplateDialog;

public class ReportTemplateDialogOutput : IDialogContentOutput
{
	public required ReportTemplateDialogResult DialogResult { get; init; }
	public List<LabTestReportTemplate>? ReportTemplates { get; init; }
}