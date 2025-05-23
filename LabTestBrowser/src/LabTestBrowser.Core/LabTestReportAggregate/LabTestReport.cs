using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.Core.LabTestReportAggregate;

public class LabTestReport : EntityBase, IAggregateRoot
{
	public LabTestReport(AccessionNumber accessionNumber, SpecimenCollectionCenter specimenCollectionCenter, Patient patient)
	{
		AccessionNumber = accessionNumber;
		SpecimenCollectionCenter = specimenCollectionCenter;
		Patient = patient;
	}

	private LabTestReport() { }

	public AccessionNumber AccessionNumber { get; private set; } = null!;
	public SpecimenCollectionCenter SpecimenCollectionCenter { get; private set; } = null!;
	public Patient Patient { get; private set; } = null!;

	public void UpdateSpecimenCollectionCenter(SpecimenCollectionCenter collectionCenter) => SpecimenCollectionCenter = collectionCenter;
	public void UpdatePatient(Patient patient) => Patient = patient;
}