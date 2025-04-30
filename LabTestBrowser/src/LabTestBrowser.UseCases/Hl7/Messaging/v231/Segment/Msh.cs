namespace LabTestBrowser.UseCases.Hl7.Messaging.v231.Segment;

public record Msh
{
	public string? FieldSeparator { get; init; }
	public string? EncodingCharacters { get; init; }
	public string? SendingApplication { get; init; }
	public string? SendingFacility { get; init; }
	public string? ReceivingApplication { get; init; }
	public string? ReceivingFacility { get; init; }
	public string? DateTimeOfMessage { get; init; }
	public string? Security { get; init; }
	public required string MessageType { get; init; }
	public required string MessageControlId { get; init; }
	public string? ProcessingId { get; init; }
	public required string VersionId { get; init; }
	public string? SequenceNumber { get; init; }
	public string? ContinuationPointer { get; init; }
	public string? AcceptAcknowledgmentType { get; init; }
	public string? ApplicationAcknowledgmentType { get; init; }
	public string? CountryCode { get; init; }
	public string? CharacterSet { get; init; }
	public string? PrincipalLanguageOfMessage { get; init; }
	public string? AlternateCharacterSetHandlingScheme { get; init; }
}