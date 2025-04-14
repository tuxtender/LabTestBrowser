using LabTestBrowser.Core.LabTestReportTemplateAggregate;

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

		//TODO: Add custom exception
		throw new Exception($"Template engine not found for file format {templateFileFormat}");
	}
}