namespace LabTestBrowser.Core.PatientAggregate;

public class Patient : EntityBase, IAggregateRoot
{
	public string? HealthcareProxy { get; private set; }
	public string? Name { get; private set; }
	public string Animal { get; private set; }
	public Gender Gender { get; private set; } = Gender.None;
	public string? Breed { get; private set; }
	public DateOnly? BirthDate { get; private set; }

	private Patient(string animal)
	{
		Animal = Guard.Against.NullOrEmpty(animal, nameof(animal));
	}

	public static Result<Patient> Create(string animal, string? healthcareProxy, string? name, DateOnly? birthDate)
	{
		if (string.IsNullOrEmpty(healthcareProxy) && string.IsNullOrEmpty(name) && !birthDate.HasValue)
			return Result.Error("Patient details are inappropriate");

		return Result.Success(new Patient(animal));
	}

	//TODO: Constrains
	public void UpdateAnimal(string animal) => Animal = animal;
	public void UpdateHealthcareProxy(string? healthcareProxy) => HealthcareProxy = healthcareProxy;
	public void UpdateName(string? name) => Name = name;
	public void UpdateBirthDate(DateOnly? birthDate) => BirthDate = birthDate;
	
	public void SetGender(Gender gender) => Gender = gender;
	public void SetBreed(string? breed) => Breed = breed;
}