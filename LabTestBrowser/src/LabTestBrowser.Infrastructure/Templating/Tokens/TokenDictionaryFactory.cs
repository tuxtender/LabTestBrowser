using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public class TokenDictionaryFactory(LabTestReportTokensFactory tokensFactory) : ITokenDictionaryFactory
{
	public IReadOnlyDictionary<string, string> Create(LabTestReport report, CompleteBloodCount? completeBloodCount, string? testName)
	{
		return tokensFactory.Create(report, completeBloodCount, testName)
			.ToDictionary(token => token.Name, token => token.FormattedValue);
	}
}