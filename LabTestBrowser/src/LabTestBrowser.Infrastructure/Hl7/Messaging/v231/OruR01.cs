using LabTestBrowser.Infrastructure.Hl7.Messaging.v231.Segment;

namespace LabTestBrowser.Infrastructure.Hl7.Messaging.v231;

public record OruR01
{
	public required Msh Msh { get; init; }
	public required Obr Obr { get; init; }
	public required IReadOnlyList<Obx> ObxList { get; init; }
}