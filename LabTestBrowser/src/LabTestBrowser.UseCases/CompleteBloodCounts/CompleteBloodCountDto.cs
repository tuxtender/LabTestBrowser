namespace LabTestBrowser.UseCases.CompleteBloodCounts;

public record CompleteBloodCountDto
{
	public string? ExternalId { get; init; }
	public DateTime ObservationDateTime { get; init; }
	public string? WhiteBloodCell { get; init; }
	public string? LymphocytePercent { get; init; }
	public string? MonocytePercent { get; init; }
	public string? NeutrophilPercent { get; init; }
	public string? EosinophilPercent { get; init; }
	public string? BasophilPercent { get; init; }
	public string? RedBloodCell { get; init; }
	public string? Hemoglobin { get; init; }
	public string? Hematocrit { get; init; }
	public string? MeanCorpuscularVolume { get; init; }
	public string? MeanCorpuscularHemoglobin { get; init; }
	public string? MeanCorpuscularHemoglobinConcentration { get; init; }
	public string? RedBloodCellDistributionWidth { get; init; }
	public string? Platelet { get; init; }
	public string? MeanPlateletVolume { get; init; }
}