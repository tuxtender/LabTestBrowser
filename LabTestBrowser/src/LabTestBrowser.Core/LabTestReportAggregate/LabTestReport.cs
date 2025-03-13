namespace LabTestBrowser.Core.LabTestReportAggregate;

public class LabTestReport(Specimen specimen, SpecimenCollectionCenter specimenCollectionCenter, Patient patient)
	: EntityBase, IAggregateRoot
{
	public Specimen Specimen { get; private set; } = Guard.Against.Null(specimen, nameof(specimen));

	public SpecimenCollectionCenter SpecimenCollectionCenter { get; private set; } =
		Guard.Against.Null(specimenCollectionCenter, nameof(specimenCollectionCenter));

	public Patient Patient { get; private set; } = Guard.Against.Null(patient, nameof(patient));

	public int? CompleteBloodCountId { get; private set; }

	public void SetCompleteBloodCount(int completeBloodCountId) => CompleteBloodCountId = completeBloodCountId;
}