namespace LabTestBrowser.Core.LabTestReportAggregate;

public class LabTestReport(Specimen specimen, SpecimenCollectionCenter specimenCollectionCenter, int patientId) : EntityBase, IAggregateRoot
{
	public Specimen Specimen { get; private set; } = Guard.Against.Null(specimen, nameof(specimen));

	public SpecimenCollectionCenter SpecimenCollectionCenter { get; private set; } =
		Guard.Against.Null(specimenCollectionCenter, nameof(specimenCollectionCenter));

	public int PatientId { get; private set; } = Guard.Against.Null(patientId, nameof(patientId));

	public int? CompleteBloodCountId { get; private set; }

	public void SetCompleteBloodCount(int completeBloodCountId) => CompleteBloodCountId = completeBloodCountId;
}