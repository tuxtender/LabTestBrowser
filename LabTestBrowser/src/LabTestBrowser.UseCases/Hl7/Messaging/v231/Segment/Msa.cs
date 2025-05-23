namespace LabTestBrowser.UseCases.Hl7.Messaging.v231.Segment;

public record Msa
{
	public required string AcknowledgementCode { get; init; }
	public required string MessageControlId { get; init; }
	public string? TextMessage { get; init; }
	public string? ExpectedSequenceNumber { get; init; }
	public string? DelayedAcknowledgmentType { get; init; }
	public string? ErrorCondition { get; init; }
}