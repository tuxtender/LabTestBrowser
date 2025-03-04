namespace LabTestBrowser.Core.PatientAggregate;

public class Gender(string name, Sex sex) : ValueObject
{
	public static Gender None => new(string.Empty, Sex.None);
	public string Name { get; private set; } = name;
	public Sex Sex { get; private set; } = sex;

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Name;
		yield return Sex;
	}
}