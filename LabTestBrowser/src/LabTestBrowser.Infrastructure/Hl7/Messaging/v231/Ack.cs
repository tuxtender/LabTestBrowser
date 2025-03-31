using LabTestBrowser.Infrastructure.Hl7.Messaging.v231.Segment;

namespace LabTestBrowser.Infrastructure.Hl7.Messaging.v231;

public record Ack
{
	public required Msh Msh { get; init; }
	public required Msa Msa { get; init; }
}