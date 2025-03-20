using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.UI;

public class LabRequisitionViewModel : BaseViewModel
{
	private DateOnly _date;
	private int _specimen;

	private string? _facility;
	private string? _tradeName;

	private string? _animal;
	private string? _petOwner;
	private string? _nickname;
	private string? _breed;
	private string? _category;

	private int? _ageInYears;
	private int? _ageInMonths;
	private int? _ageInDays;

	public int Id { get; internal set; }

	public DateOnly Date
	{
		get => _date;
		set => SetField(ref _date, value);
	}

	public int Specimen
	{
		get => _specimen;
		set => SetField(ref _specimen, value);
	}

	public string? Facility
	{
		get => _facility;
		set => SetField(ref _facility, value);
	}

	public string? TradeName
	{
		get => _tradeName;
		set => SetField(ref _tradeName, value);
	}

	public string? Animal
	{
		get => _animal;
		set => SetField(ref _animal, value);
	}

	public string? PetOwner
	{
		get => _petOwner;
		set => SetField(ref _petOwner, value);
	}

	public string? Nickname
	{
		get => _nickname;
		set => SetField(ref _nickname, value);
	}

	public string? Breed
	{
		get => _breed;
		set => SetField(ref _breed, value);
	}

	public string? Category
	{
		get => _category;
		set => SetField(ref _category, value);
	}

	public int? AgeInYears
	{
		get => _ageInYears;
		set => SetField(ref _ageInYears, value);
	}

	public int? AgeInMonths
	{
		get => _ageInMonths;
		set => SetField(ref _ageInMonths, value);
	}

	public int? AgeInDays
	{
		get => _ageInDays;
		set => SetField(ref _ageInDays, value);
	}

	public void SetLabRequisition(LabTestReportDTO report) 
	{
		Id = report.Id;
		Date = report.Date;
		Specimen = report.SpecimenSequentialNumber;
		Facility = report.Facility;
		TradeName = report.TradeName;
		Animal = report.Animal;
		PetOwner = report.HealthcareProxy;
		Nickname = report.Name;
		Breed = report.Breed;
		Category = report.Category;
		AgeInYears = report.AgeInYears;
		AgeInMonths = report.AgeInMonths;
		AgeInDays = report.AgeInDays;
	}
}