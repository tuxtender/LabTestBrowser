namespace LabTestBrowser.UseCases.Hl7.Exceptions;

public class UnsupportedHl7MessageException : Exception
{
	public UnsupportedHl7MessageException(string messageControlId, string messageType)
	{
		MessageControlId = messageControlId;
		MessageType = messageType;
	}

	public UnsupportedHl7MessageException(string messageControlId, string messageType, string message)
		: base(message)
	{
		MessageControlId = messageControlId;
		MessageType = messageType;
	}

	public UnsupportedHl7MessageException(string messageControlId, string messageType, string message, Exception inner)
		: base(message, inner)
	{
		MessageControlId = messageControlId;
		MessageType = messageType;
	}

	public string MessageControlId { get; }
	public string MessageType { get; }
}