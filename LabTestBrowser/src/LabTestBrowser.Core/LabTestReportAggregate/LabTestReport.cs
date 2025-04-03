namespace LabTestBrowser.Core.LabTestReportAggregate;

public class LabTestReport
	: EntityBase, IAggregateRoot
{
	public LabTestReport(Specimen specimen, SpecimenCollectionCenter specimenCollectionCenter, Patient patient)
	{
		Specimen = specimen;
		SpecimenCollectionCenter = specimenCollectionCenter;
		Patient = patient;
	}
	
	private LabTestReport() { }

	public Specimen Specimen { get; private set; } = null!;

	public SpecimenCollectionCenter SpecimenCollectionCenter { get; private set; } = null!;

	public Patient Patient { get; private set; } = null!;

	public int? CompleteBloodCountId { get; private set; }

	public void SetSpecimenCollectionCenter(SpecimenCollectionCenter collectionCenter) => SpecimenCollectionCenter = collectionCenter;

	public void SetPatient(Patient patient) => Patient = patient;
	public void SetCompleteBloodCount(int? completeBloodCountId) => CompleteBloodCountId = completeBloodCountId;
}