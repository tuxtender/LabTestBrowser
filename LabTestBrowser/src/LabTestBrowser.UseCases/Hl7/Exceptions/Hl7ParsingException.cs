namespace LabTestBrowser.UseCases.Hl7.Exceptions;

public class Hl7ParsingException : Exception
{
	public Hl7ParsingException() { }

	public Hl7ParsingException(string message)
		: base(message) { }

	public Hl7ParsingException(string message, Exception inner)
		: base(message, inner) { }
}