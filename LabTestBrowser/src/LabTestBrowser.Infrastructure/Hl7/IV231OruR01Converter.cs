using LabTestBrowser.Infrastructure.Hl7.Messaging.v231;

namespace LabTestBrowser.Infrastructure.Hl7;

public interface IV231OruR01Converter
{
	OruR01 Convert(string hl7Message);
}