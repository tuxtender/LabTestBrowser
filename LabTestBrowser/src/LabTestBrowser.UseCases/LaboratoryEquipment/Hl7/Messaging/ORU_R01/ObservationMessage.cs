namespace LabTestBrowser.UseCases.Hl7.Messaging.ORU_R01;

public record ObservationMessage
{
	public required MessageHeader MessageHeader { get; init; }
	public required ObservationRequest ObservationRequest { get; init; }
	public required IReadOnlyList<ObservationResult> ObservationResults { get; init; }
}