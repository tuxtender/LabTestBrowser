namespace LabTestBrowser.UseCases.LaboratoryEquipment.Hl7.Dto;

public record Urit5160CompleteBloodCount
{
	public required string ExternalId { get; init; }
	public DateTime ObservationDateTime { get; init; }
	public required string WhiteBloodCell { get; init; }
	public required string LymphocytePercent { get; init; }
	public required string MonocytePercent { get; init; }
	public required string NeutrophilPercent { get; init; }
	public required string EosinophilPercent { get; init; }
	public required string BasophilPercent { get; init; }
	public required string RedBloodCell { get; init; }
	public required string Hemoglobin { get; init; }
	public required string Hematocrit { get; init; }
	public required string MeanCorpuscularVolume { get; init; }
	public required string MeanCorpuscularHemoglobin { get; init; }
	public required string MeanCorpuscularHemoglobinConcentration { get; init; }
	public required string RedBloodCellDistributionWidth { get; init; }
	public required string Platelet { get; init; }
	public required string MeanPlateletVolume { get; init; }
}