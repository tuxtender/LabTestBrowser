namespace LabTestBrowser.Core.Common;

public sealed class ValidationErrorCode
{
	private ValidationErrorCode(string code)
	{
		Code = code;
	}

	public string Code { get; }

	public static ValidationErrorCode Required(string className, string propertyName) => new($"{className}.{propertyName}.Required");

	public static ValidationErrorCode OutOfRange(string className, string propertyName) => new($"{className}.{propertyName}.OutOfRange");

	public static ValidationErrorCode InsufficientData(string className, string groupName) =>
		new($"{className}.{groupName}.InsufficientData");
}