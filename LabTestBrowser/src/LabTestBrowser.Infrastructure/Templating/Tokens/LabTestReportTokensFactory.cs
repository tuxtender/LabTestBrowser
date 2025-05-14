using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;

namespace LabTestBrowser.Infrastructure.Templating.Tokens;

public class LabTestReportTokensFactory
{
	private readonly ITokenFormatter<DateOnly> _dateFormatter = new DateTokenFormatter();
	private readonly ITokenFormatter<string?> _textFormatter = new TextTokenFormatter();
	private readonly ITokenFormatter<int?> _numberFormatter = new NumberTokenFormatter();
	private readonly IAgeTokenFormatter _ageFormatter = new AgeTokenFormatter();
	private readonly IPersonNameTokenFormatter _nameFormatter = new PersonNameTokenFormatter();

	public IReadOnlyList<ITemplateToken> Create(LabTestReport report, CompleteBloodCount? cbc, string? testName)
	{
		return new List<ITemplateToken>
		{
			new TemplateToken<DateOnly>("DATE", report.AccessionNumber.Date, _dateFormatter),
			new TemplateToken<int?>("SAMPLE", report.AccessionNumber.SequenceNumber, _numberFormatter),
			new TemplateToken<string?>("CLINIC", report.SpecimenCollectionCenter.Facility, _textFormatter),
			new TemplateToken<string?>("SUBSIDIARY", report.SpecimenCollectionCenter.TradeName, _textFormatter),
			new TemplateToken<string?>("ANIMAL", report.Patient.Animal, _textFormatter),
			new TemplateToken<string?>("OWNER", report.Patient.HealthcareProxy, _textFormatter),
			new TemplateToken<string?>("OWNER.SECOND_NAME", report.Patient.HealthcareProxy, _nameFormatter),
			new TemplateToken<string?>("NICKNAME", report.Patient.Name, _textFormatter),
			new TemplateToken<string?>("BREED", report.Patient.Breed, _textFormatter),
			new TemplateToken<string?>("SEX", report.Patient.Category, _textFormatter),
			new TemplateToken<Age>("AGE", report.Patient.Age, _ageFormatter),
			new TemplateToken<int?>("AGE.YEAR", report.Patient.Age.Years, _numberFormatter),
			new TemplateToken<int?>("AGE.MONTH", report.Patient.Age.Months, _numberFormatter),
			new TemplateToken<int?>("AGE.DAY", report.Patient.Age.Days, _numberFormatter),
			new TemplateToken<string?>("LAB_TEST", testName, _textFormatter),
			new TemplateToken<string?>("WBC", cbc?.WhiteBloodCell?.Value, _textFormatter),
			new TemplateToken<string?>("LYM%", cbc?.LymphocytePercent?.Value, _textFormatter),
			new TemplateToken<string?>("MON%", cbc?.MonocytePercent?.Value, _textFormatter),
			new TemplateToken<string?>("NEU%", cbc?.NeutrophilPercent?.Value, _textFormatter),
			new TemplateToken<string?>("EOS%", cbc?.EosinophilPercent?.Value, _textFormatter),
			new TemplateToken<string?>("BASO%", cbc?.BasophilPercent?.Value, _textFormatter),
			new TemplateToken<string?>("RBC", cbc?.RedBloodCell?.Value, _textFormatter),
			new TemplateToken<string?>("HGB", cbc?.Hemoglobin?.Value, _textFormatter),
			new TemplateToken<string?>("HCT", cbc?.Hematocrit?.Value, _textFormatter),
			new TemplateToken<string?>("MCV", cbc?.MeanCorpuscularVolume?.Value, _textFormatter),
			new TemplateToken<string?>("MCH", cbc?.MeanCorpuscularHemoglobin?.Value, _textFormatter),
			new TemplateToken<string?>("MCHC", cbc?.MeanCorpuscularHemoglobinConcentration?.Value, _textFormatter),
			new TemplateToken<string?>("RDW-CV", cbc?.RedBloodCellDistributionWidth?.Value, _textFormatter),
			new TemplateToken<string?>("PLT", cbc?.Platelet?.Value, _textFormatter),
			new TemplateToken<string?>("MPV", cbc?.MeanPlateletVolume?.Value, _textFormatter),
		};
	}
}