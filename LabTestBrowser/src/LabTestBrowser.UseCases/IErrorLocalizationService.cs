namespace LabTestBrowser.UseCases;

public interface IErrorLocalizationService
{
	string ApplicationFault { get;}
	string LabTestReportIdConflict { get;}
	string ExportFailed { get;}
	string TestNotSelected { get;}
	string LabTestReportRequired { get;}
	string LabTestReportNotSaved { get;}
}