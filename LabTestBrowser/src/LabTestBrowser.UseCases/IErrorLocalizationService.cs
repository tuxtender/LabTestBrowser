namespace LabTestBrowser.UseCases;

public interface IErrorLocalizationService
{
	string GetApplicationFault();
	string GetLabTestReportIdConflict();
	string GetExportFailed();
	string GetTestNotSelected();
	string GetLabTestReportRequired();
	string GetLabTestReportNotSaved();
}