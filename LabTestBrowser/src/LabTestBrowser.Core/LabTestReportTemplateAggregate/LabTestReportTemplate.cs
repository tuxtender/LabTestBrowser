namespace LabTestBrowser.Core.LabTestReportTemplateAggregate;

public class LabTestReportTemplate( string facility, string tradeName, string animal, string title, string path) : EntityBase, IAggregateRoot
{
	public string Facility { get; private set; } = facility;
	public string TradeName { get; private set; } = tradeName;
	public string Animal { get; private set; } = animal;
	public string Title { get; private set; } = title;
	public string Path { get; private set; } = path;
}