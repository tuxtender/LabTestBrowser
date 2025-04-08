using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.UI.Dialogs.ReportTemplateDialog;

public class ReportTemplateDialogInput: IDialogContentInput
{
	public required IEnumerable<LabTestReportTemplate> ReportTemplates { get; init; }
}