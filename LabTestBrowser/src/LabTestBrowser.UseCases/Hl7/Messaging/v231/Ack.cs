using LabTestBrowser.UseCases.Hl7.Messaging.v231.Segment;

namespace LabTestBrowser.UseCases.Hl7.Messaging.v231;

public record Ack
{
	public required Msh Msh { get; init; }
	public required Msa Msa { get; init; }
}