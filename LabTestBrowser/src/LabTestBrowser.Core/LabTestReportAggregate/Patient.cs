namespace LabTestBrowser.Core.LabTestReportAggregate;

public class Patient : ValueObject
{
	private Patient() { }

	public string? HealthcareProxy { get; init; }
	public string? Name { get; init; }
	public required string Animal { get; init; }
	public string? Category { get; init; }
	public string? Breed { get; init; }
	public Age Age { get; private set; } = Age.None;

	public static Result<Patient> Create(string animal, Age age, string? healthcareProxy, string? name, string? category, string? breed)
	{
		if (string.IsNullOrWhiteSpace(animal))
			return Result.Error("An animal specification required");

		var isIncomplete = age == Age.None && string.IsNullOrEmpty(healthcareProxy) && string.IsNullOrEmpty(name);

		if (isIncomplete)
			return Result.Error("Patient details are incomplete");

		var patient = new Patient
		{
			HealthcareProxy = healthcareProxy,
			Name = name,
			Age = age,
			Animal = animal,
			Category = category,
			Breed = breed
		};

		return Result.Success(patient);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return HealthcareProxy ?? string.Empty;
		yield return Name ?? string.Empty;
		yield return Animal;
		yield return Category ?? string.Empty;
		yield return Breed ?? string.Empty;
		yield return Age;
	}
}