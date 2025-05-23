namespace LabTestBrowser.UseCases.LabTestReports.Save;

public record SaveLabTestReportCommand : ICommand<Result<LabTestReportDto>>
{
	public int? Id { get; init; }
	public int OrderNumber { get; init; }
	public DateOnly OrderDate { get; init; }
	public string? Facility { get; init; }
	public string? TradeName { get; init; }
	public string? PetOwner { get; init; }
	public string? Nickname { get; init; }
	public string? Animal { get; init; }
	public string? Category { get; init; }
	public string? Breed { get; init; }
	public int? AgeInYears { get; init; }
	public int? AgeInMonths { get; init; }
	public int? AgeInDays { get; init; }
	public int? CompleteBloodCountId { get; init; }
}