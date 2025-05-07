using LabTestBrowser.UI.Dialogs;

namespace LabTestBrowser.UI.LabResult.ReportExportDialog;

public record ReportExportDialogInput(int? LabTestReportId) : IDialogContentInput;