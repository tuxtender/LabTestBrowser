namespace LabTestBrowser.Core.CompleteBloodCountAggregate;

public class LabTestResult(string value) : ValueObject
{
	public string Value { get; } = value;

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}