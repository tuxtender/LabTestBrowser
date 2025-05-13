using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.UseCases.Export.Exceptions;

namespace LabTestBrowser.Infrastructure.Export;

public class TemplateEngineResolver(IExcelTemplateEngine excelTemplateEngine, IWordTemplateEngine wordTemplateEngine)
	: ITemplateEngineResolver
{
	private readonly Dictionary<TemplateFileFormat, IFileTemplateEngine> _templateEngines = new()
	{
		{
			TemplateFileFormat.Excel, excelTemplateEngine
		},
		{
			TemplateFileFormat.Word, wordTemplateEngine
		}
	};

	public IFileTemplateEngine ResolveByFileFormat(TemplateFileFormat templateFileFormat)
	{
		if (_templateEngines.TryGetValue(templateFileFormat, out var templateEngine))
			return templateEngine;

		throw new TemplateEngineNotFoundException();
	}
}