namespace LabTestBrowser.UseCases;

public static class ValidationLocalizationServiceExtensions
{
	public static IEnumerable<ValidationError>
		Localize(this IValidationLocalizationService localizer, IEnumerable<ValidationError> errors) =>
		errors.Select(error => new ValidationError(localizer.GetString(error.Identifier)));
}