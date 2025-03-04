namespace LabTestBrowser.Core.LabTestReportAggregate;

public class Specimen(int sequentialNumber, DateOnly date) : ValueObject
{
	public int SequentialNumber { get; private set; } = Guard.Against.NegativeOrZero(sequentialNumber);
	public DateOnly Date { get; private set; } = date;

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return SequentialNumber;
		yield return Date;
	}
}