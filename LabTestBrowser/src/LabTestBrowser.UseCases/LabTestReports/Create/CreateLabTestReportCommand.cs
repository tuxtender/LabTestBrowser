namespace LabTestBrowser.UseCases.LabTestReports.Create;

public record CreateLabTestReportCommand : ICommand<Result<LabTestReportDto>>
{
	public int SequenceNumber { get; init; }
	public DateOnly Date { get; init; }
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
}