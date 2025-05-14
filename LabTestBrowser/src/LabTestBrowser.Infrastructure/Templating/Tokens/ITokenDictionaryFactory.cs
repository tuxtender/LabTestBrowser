using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public interface ITokenDictionaryFactory
{
	IReadOnlyDictionary<string, string> Create(LabTestReport report, CompleteBloodCount? completeBloodCount, string? testName);
}