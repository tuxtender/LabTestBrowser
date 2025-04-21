namespace LabTestBrowser.UseCases.SpecimenCollectionCenters;

public record SpecimenCollectionCenterDto
{
	public string? Facility { get; init; }
	public IEnumerable<string> TradeNames { get; init; } = [];
}