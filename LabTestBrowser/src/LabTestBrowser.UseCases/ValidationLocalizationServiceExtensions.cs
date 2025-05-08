namespace LabTestBrowser.UseCases;

public static class ValidationLocalizationServiceExtensions
{
	public static IEnumerable<ValidationError>
		Localize(this IValidationLocalizationService localizer, IEnumerable<ValidationError> errors) =>
		errors.Select(error => new ValidationError
		{
			Identifier = error.Identifier,
			ErrorMessage = localizer.GetString(error.ErrorCode),
			ErrorCode = error.ErrorCode,
			Severity = error.Severity
		});
}