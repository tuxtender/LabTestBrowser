using LabTestBrowser.Core.PatientAggregate;

namespace LabTestBrowser.Core.LabTestReportAggregate.Exceptions;

public class UnsupportedAnimalGenderException : Exception
{
	public UnsupportedAnimalGenderException(Patient patient)
		: base($"Unsupported gender {patient.Animal} of animal {patient.Gender?.Name}") { }
}