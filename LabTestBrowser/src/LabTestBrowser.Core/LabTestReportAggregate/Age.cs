namespace LabTestBrowser.Core.LabTestReportAggregate;

public class Age : ValueObject
{
	public static readonly Age None = new(null, null, null);

	private Age(int? years, int? months, int? days) 
	{
		Years = years;
		Months = months;
		Days = days;
	}
	public int? Years { get; private set; }
	public int? Months { get; private set; }
	public int? Days { get; private set; }

	public static Result<Age> Create(int? years, int? months, int? days)
	{
		var isAgeAvailable = years.HasValue || months.HasValue || days.HasValue;

		return isAgeAvailable ? new Age(years, months, days) : None;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Years ?? 0;
		yield return Months ?? 0;
		yield return Days ?? 0;
	}
}