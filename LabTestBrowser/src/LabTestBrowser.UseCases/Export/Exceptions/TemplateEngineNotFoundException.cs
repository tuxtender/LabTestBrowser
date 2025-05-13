namespace LabTestBrowser.UseCases.Export.Exceptions;

public class TemplateEngineNotFoundException : Exception
{
	public TemplateEngineNotFoundException()
		: base("Template engine not registered") { }

	public TemplateEngineNotFoundException(Exception inner)
		: base("Template engine not registered", inner) { }
}