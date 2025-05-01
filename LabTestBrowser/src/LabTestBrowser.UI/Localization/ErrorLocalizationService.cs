using LabTestBrowser.UseCases;

namespace LabTestBrowser.UI.Localization;

public class ErrorLocalizationService : IErrorLocalizationService
{
	public string ApplicationFault => Resources.Errors.ApplicationFault;
	public string LabTestReportIdConflict => Resources.Errors.LabTestReportIdConflict;
	public string ExportFailed => Resources.Errors.ExportFailed;
	public string TestNotSelected => Resources.Errors.TestNotSelected;
	public string LabTestReportRequired => Resources.Errors.LabTestReportRequired;
	public string LabTestReportNotSaved => Resources.Errors.LabTestReportNotSaved;
}