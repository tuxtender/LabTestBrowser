namespace LabTestBrowser.Core.CompleteBloodCountAggregate;

public class AccessionNumber : ValueObject
{
	private AccessionNumber(int sequenceNumber, DateOnly date)
	{
		SequenceNumber = sequenceNumber;
		Date = date;
	}

	public int SequenceNumber { get; private set; }
	public DateOnly Date { get; private set; }

	public static Result<AccessionNumber> Create(int sequenceNumber, DateOnly date)
	{
		if (sequenceNumber < 1)
			return Result.Error();

		return new AccessionNumber(sequenceNumber, date);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return SequenceNumber;
		yield return Date;
	}
}