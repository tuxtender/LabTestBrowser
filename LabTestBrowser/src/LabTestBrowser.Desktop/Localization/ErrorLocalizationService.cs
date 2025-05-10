using LabTestBrowser.UseCases;

namespace LabTestBrowser.Desktop.Localization;

public class ErrorLocalizationService : IErrorLocalizationService
{
	public string ApplicationFault => UI.Resources.Errors.ApplicationFault;
	public string LabTestReportIdConflict => UI.Resources.Errors.LabTestReportIdConflict;
	public string ExportFailed => UI.Resources.Errors.ExportFailed;
	public string TestNotSelected => UI.Resources.Errors.TestNotSelected;
	public string LabTestReportRequired => UI.Resources.Errors.LabTestReportRequired;
	public string LabTestReportNotSaved => UI.Resources.Errors.LabTestReportNotSaved;
}