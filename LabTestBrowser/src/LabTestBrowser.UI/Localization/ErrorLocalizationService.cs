using LabTestBrowser.UseCases;

namespace LabTestBrowser.UI.Localization;

public class ErrorLocalizationService : IErrorLocalizationService
{
	public string GetApplicationFault() => Resources.Errors.ApplicationFault;
	public string GetLabTestReportIdConflict() => Resources.Errors.LabTestReportIdConflict;
	public string GetExportFailed() => Resources.Errors.ExportFailed;
	public string GetTestNotSelected() => Resources.Errors.TestNotSelected;
	public string GetLabTestReportRequired() => Resources.Errors.LabTestReportRequired;
	public string GetLabTestReportNotSaved() => Resources.Errors.LabTestReportNotSaved;
}