using LabTestBrowser.Desktop.Dialogs;

namespace LabTestBrowser.Desktop.LabResult.ReportExportDialog;

public record ReportExportDialogInput(int? LabTestReportId) : IDialogContentInput;