using LabTestBrowser.UseCases.Hl7.Messaging.v231;

namespace LabTestBrowser.UseCases.Hl7;

public interface IV231OruR01Converter
{
	OruR01 Convert(string hl7Message);
}