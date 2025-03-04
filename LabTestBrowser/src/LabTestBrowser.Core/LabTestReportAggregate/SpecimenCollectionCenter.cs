namespace LabTestBrowser.Core.LabTestReportAggregate;

public class SpecimenCollectionCenter(string facility, string tradeName) : ValueObject
{
	public string Facility { get; private set; } = Guard.Against.NullOrEmpty(facility, nameof(facility));
	public string? TradeName { get; private set; } = tradeName;

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Facility;
		yield return TradeName ?? string.Empty;
	}
}