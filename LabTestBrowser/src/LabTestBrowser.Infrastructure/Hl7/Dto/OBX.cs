namespace LabTestBrowser.Infrastructure.Hl7.Dto;

public record OBX
{
	public string? Id { get; init; }
	public string? ValueType { get; init; }
	public string? ObservationIdentifier { get; init; }
	public string? ObservationSubId { get; init; }
	public string? ObservationValue { get; init; }
	public string? Units { get; init; }
	public string? ReferencesRange { get; init; }
	public string? AbnormalFlags { get; init; }
	public string? Probability { get; init; }
	public string? NatureOfAbnormalTest { get; init; }
	public string? ObservationResultStatus { get; init; }
	public string? DateLastObsNormalValues { get; init; }
	public string? UserDefinedAccessChecks { get; init; }
	public string? DateTimeOfTheObservation { get; init; }
	public string? ProducerId { get; init; }
	public string? ResponsibleObserver { get; init; }
	public string? ObservationMethod { get; init; }
}