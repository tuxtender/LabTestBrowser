using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.LabTestReports.Export.Format;

namespace LabTestBrowser.UseCases.LabTestReports.Export;

public class LabTestReportTokens(LabTestReport report, CompleteBloodCount? cbc)
{
	public IReadOnlyList<IToken> Tokens { get; init; } = new List<IToken>()
	{
		new DateToken("DATE", report.AccessionNumber.Date),
		new NumberToken("SAMPLE", report.AccessionNumber.SequenceNumber),
		new TextToken("CLINIC", report.SpecimenCollectionCenter.Facility),
		new TextToken("SUBSIDIARY", report.SpecimenCollectionCenter.TradeName),
		new TextToken("ANIMAL", report.Patient.Animal),
		new TextToken("OWNER", report.Patient.HealthcareProxy),
		new PersonNameToken("OWNER.SECOND_NAME", report.Patient.HealthcareProxy),
		new TextToken("NICKNAME", report.Patient.Name),
		new TextToken("BREED", report.Patient.Breed),
		new TextToken("SEX", report.Patient.Category),
		new AgeToken("AGE", report.Patient.Age),
		new NumberToken("AGE.YEAR", report.Patient.Age.Years),
		new NumberToken("AGE.MONTH", report.Patient.Age.Months),
		new NumberToken("AGE.DAY", report.Patient.Age.Days),
		new TextToken("LAB_TEST", "CBC"), //TODO
		new TextToken("WBC", cbc?.WhiteBloodCell?.Value),
		new TextToken("LYM%", cbc?.LymphocytePercent?.Value),
		new TextToken("MON%", cbc?.MonocytePercent?.Value),
		new TextToken("NEU%", cbc?.NeutrophilPercent?.Value),
		new TextToken("EOS%", cbc?.EosinophilPercent?.Value),
		new TextToken("BASO%", cbc?.BasophilPercent?.Value),
		new TextToken("RBC", cbc?.RedBloodCell?.Value),
		new TextToken("HGB", cbc?.Hemoglobin?.Value),
		new TextToken("HCT", cbc?.Hematocrit?.Value),
		new TextToken("MCV", cbc?.MeanCorpuscularVolume?.Value),
		new TextToken("MCH", cbc?.MeanCorpuscularHemoglobin?.Value),
		new TextToken("MCHC", cbc?.MeanCorpuscularHemoglobinConcentration?.Value),
		new TextToken("RDW-CV", cbc?.RedBloodCellDistributionWidth?.Value),
		new TextToken("PLT", cbc?.Platelet?.Value),
		new TextToken("MPV", cbc?.MeanPlateletVolume?.Value),
	};
}