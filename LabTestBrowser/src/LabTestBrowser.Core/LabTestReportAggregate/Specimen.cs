namespace LabTestBrowser.Core.LabTestReportAggregate;

public class Specimen(int sequentialNumber, DateOnly observationDate) : ValueObject
{
	public int SequentialNumber { get; private set; } = Guard.Against.NegativeOrZero(sequentialNumber);
	public DateOnly ObservationDate { get; private set; } = observationDate;

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return SequentialNumber;
		yield return ObservationDate;
	}
}