namespace LabTestBrowser.Infrastructure.Hl7.Messaging.v231.Segment;

public class Obr
{
	public int? Id { get; init; }
	public string? PlacerOrderNumber { get; init; }
	public string? FillerOrderNumber { get; init; }
	public string? UniversalServiceId { get; init; }
	public string? Priority { get; init; }
	public DateTime? RequestedDatetime { get; init; }
	public DateTime? ObservationDateTime { get; init; }
	public DateTime? ObservationEndDateTime { get; init; }
	public string? CollectionVolume { get; init; }
	public string? CollectorIdentifier { get; init; }
	public string? SpecimenActionCode { get; init; }
	public string? DangerCode { get; init; }
	public string? RelevantClinicalInfo { get; init; }
	public DateTime? SpecimenReceivedDateTime { get; init; }
	public string? SpecimenSource { get; init; }
	public string? OrderingProvider { get; init; }
	public string? OrderCallbackPhoneNumber { get; init; }
	public string? PlacerField1 { get; init; }
	public string? PlacerField2 { get; init; }
	public string? FillerField1 { get; init; }
	public string? FillerField2 { get; init; }
	public DateTime? ResultsReportedOrStatusChangedDateTime { get; init; }
	public string? ChargeToPractice { get; init; }
	public string? DiagnosticServSectId { get; init; }
	public string? ResultStatus { get; init; }
	public string? ParentResult { get; init; }
	public string? QuantityAndTiming { get; init; }
	public string? ResultCopiesTo { get; init; }
	public string? Parent { get; init; }
	public string? TransportationMode { get; init; }
	public string? ReasonForStudy { get; init; }
	public string? PrincipalResultInterpreter { get; init; }
	public string? AssistantResultInterpreter { get; init; }
	public string? Technician { get; init; }
	public string? Transcriptionist { get; init; }
	public DateTime? ScheduledDateTime { get; init; }
	public string? NumberOfSampleContainers { get; init; }
	public string? TransportLogisticsOfCollectedSample { get; init; }
	public string? CollectorComment { get; init; }
	public string? TransportArrangementResponsibility { get; init; }
	public string? TransportArranged { get; init; }
	public string? EscortRequired { get; init; }
	public string? PlannedPatientTransportComment { get; init; }
	public string? ProcedureCode { get; init; }
	public string? ProcedureCodeModifier { get; init; }
}