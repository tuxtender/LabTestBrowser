namespace LabTestBrowser.Desktop.LabResult.LabRequisition;

public class CollectionCenterViewModel
{
	public required string Facility { get; init; }
	public IEnumerable<string> TradeNames { get; init; } = [];
}