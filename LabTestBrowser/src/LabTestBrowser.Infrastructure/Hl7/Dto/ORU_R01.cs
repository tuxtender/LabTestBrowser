namespace LabTestBrowser.Infrastructure.Hl7.Dto;

public record ORU_R01
{
	public MSH? MessageHeader { get; init; }
	public OBR? ObservationRequest { get; init; }
	public IReadOnlyList<OBX> ObservationResult { get; init; } = new List<OBX>();
}