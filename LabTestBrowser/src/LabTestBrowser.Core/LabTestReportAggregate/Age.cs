namespace LabTestBrowser.Core.LabTestReportAggregate;

public class Age : ValueObject
{
	public static Age None = new(null, null, null);

	private Age(int? years, int? months, int? days) { }
	public int? Years { get; private set; }
	public int? Months { get; private set; }
	public int? Days { get; private set; }

	public static Result<Age> Create(int? years, int? months, int? days)
	{
		var isAgeAvailable = years.HasValue && months.HasValue && days.HasValue;

		return !isAgeAvailable ? None : new Age(years, months, days);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Years ?? 0;
		yield return Months ?? 0;
		yield return Days ?? 0;
	}
}