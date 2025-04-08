using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.UI.Dialogs.ReportTemplateDialog;

public class ReportTemplateDialogInput: IDialogContentInput
{
	public required IEnumerable<LabTestReportTemplateDto> ReportTemplates { get; init; }
}