using CommunityToolkit.Mvvm.ComponentModel;
using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.UI;

public class LabRequisitionViewModel : ObservableObject
{
	private DateOnly _labOrderDate;
	private int _labOrderNumber;

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

	public int? Id { get; internal set; }

	public DateOnly LabOrderDate
	{
		get => _labOrderDate;
		set => SetProperty(ref _labOrderDate, value);
	}

	public int LabOrderNumber
	{
		get => _labOrderNumber;
		set => SetProperty(ref _labOrderNumber, value);
	}

	public string? Facility
	{
		get => _facility;
		set => SetProperty(ref _facility, value);
	}

	public string? TradeName
	{
		get => _tradeName;
		set => SetProperty(ref _tradeName, value);
	}

	public string? Animal
	{
		get => _animal;
		set => SetProperty(ref _animal, value);
	}

	public string? PetOwner
	{
		get => _petOwner;
		set => SetProperty(ref _petOwner, value);
	}

	public string? Nickname
	{
		get => _nickname;
		set => SetProperty(ref _nickname, value);
	}

	public string? Breed
	{
		get => _breed;
		set => SetProperty(ref _breed, value);
	}

	public string? Category
	{
		get => _category;
		set => SetProperty(ref _category, value);
	}

	public int? AgeInYears
	{
		get => _ageInYears;
		set => SetProperty(ref _ageInYears, value);
	}

	public int? AgeInMonths
	{
		get => _ageInMonths;
		set => SetProperty(ref _ageInMonths, value);
	}

	public int? AgeInDays
	{
		get => _ageInDays;
		set => SetProperty(ref _ageInDays, value);
	}

	public void SetLabRequisition(LabTestReportDto report) 
	{
		Id = report.Id;
		LabOrderDate = report.Date;
		LabOrderNumber = report.SequenceNumber;
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