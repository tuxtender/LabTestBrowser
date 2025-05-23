using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160;
using LabTestBrowser.UseCases.Hl7.Messaging.v231;

namespace LabTestBrowser.UseCases.Hl7;

public static class OruR01Extensions
{
	public static string GetObservationValue(this OruR01 oru, Urit5160ObxSegment obxSegment) =>
		oru.ObxList[(int)obxSegment - 1].ObservationValue;
}