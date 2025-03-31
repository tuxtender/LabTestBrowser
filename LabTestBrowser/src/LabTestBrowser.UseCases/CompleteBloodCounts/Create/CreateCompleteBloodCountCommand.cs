namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public record CreateCompleteBloodCountCommand : ICommand<Result<int>>
{
	public string ExternalId { get; init; } = null!;
	public DateTime ObservationDateTime { get; init; }
	public string WhiteBloodCell { get; init; } = null!;
	public string Lymphocyte { get; init; } = null!;
	public string MonocytePercent { get; init; } = null!;
	public string NeutrophilPercent { get; init; } = null!;
	public string EosinophilPercent { get; init; } = null!;
	public string BasophilPercent { get; init; } = null!;
	public string RedBloodCell { get; init; } = null!;
	public string Hemoglobin { get; init; } = null!;
	public string Hematocrit { get; init; } = null!;
	public string MeanCorpuscularVolume { get; init; } = null!;
	public string MeanCorpuscularHemoglobin { get; init; } = null!;
	public string MeanCorpuscularHemoglobinConcentration { get; init; } = null!;
	public string RedBloodCellDistributionWidth { get; init; } = null!;
	public string Platelet { get; init; } = null!;
	public string MeanPlateletVolume { get; init; } = null!;
}