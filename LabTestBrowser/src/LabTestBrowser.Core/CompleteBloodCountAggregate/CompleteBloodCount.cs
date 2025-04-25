using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;

namespace LabTestBrowser.Core.CompleteBloodCountAggregate;

public class CompleteBloodCount : EntityBase, IAggregateRoot
{
	public CompleteBloodCount(string externalId, DateTime observationDatetime)
	{
		ExternalId = externalId;
		ObservationDateTime = observationDatetime;
	}

	private CompleteBloodCount() { }

	public string ExternalId { get; private set; } = null!;
	public DateTime ObservationDateTime { get; private set; }
	public AccessionNumber? AccessionNumber { get; private set; }
	public ReviewResult ReviewResult { get; private set; } = ReviewResult.UnderReview;
	public DateOnly? ReviewDate { get; private set; }
	public LabTestResult? WhiteBloodCell { get; private set; }
	public LabTestResult? LymphocytePercent { get; private set; }
	public LabTestResult? MonocytePercent { get; private set; }
	public LabTestResult? NeutrophilPercent { get; private set; }
	public LabTestResult? EosinophilPercent { get; private set; }
	public LabTestResult? BasophilPercent { get; private set; }
	public LabTestResult? RedBloodCell { get; private set; }
	public LabTestResult? Hemoglobin { get; private set; }
	public LabTestResult? Hematocrit { get; private set; }
	public LabTestResult? MeanCorpuscularVolume { get; private set; }
	public LabTestResult? MeanCorpuscularHemoglobin { get; private set; }
	public LabTestResult? MeanCorpuscularHemoglobinConcentration { get; private set; }
	public LabTestResult? RedBloodCellDistributionWidth { get; private set; }
	public LabTestResult? Platelet { get; private set; }
	public LabTestResult? MeanPlateletVolume { get; private set; }

	public void Review(AccessionNumber accessionNumber)
	{
		AccessionNumber = accessionNumber;
		ReviewResult = ReviewResult.Reported;
		ReviewDate = accessionNumber.Date;
		RegisterDomainEvent(new CompleteBloodCountReviewedEvent(Id, ReviewResult));
	}

	public void Review()
	{
		AccessionNumber = null;
		ReviewResult = ReviewResult.UnderReview;
		ReviewDate = null;
		RegisterDomainEvent(new CompleteBloodCountReviewedEvent(Id, ReviewResult));
	}

	public void Suppress(DateOnly date)
	{
		AccessionNumber = null;
		ReviewResult = ReviewResult.Suppressed;
		ReviewDate = date;
		RegisterDomainEvent(new CompleteBloodCountReviewedEvent(Id, ReviewResult));
	}

	public void SetWhiteBloodCell(string value) => WhiteBloodCell = new LabTestResult(value);
	public void SetLymphocytePercent(string value) => LymphocytePercent = new LabTestResult(value);
	public void SetMonocytePercent(string value) => MonocytePercent = new LabTestResult(value);
	public void SetNeutrophilPercent(string value) => NeutrophilPercent = new LabTestResult(value);
	public void SetEosinophilPercent(string value) => EosinophilPercent = new LabTestResult(value);
	public void SetBasophilPercent(string value) => BasophilPercent = new LabTestResult(value);
	public void SetRedBloodCell(string value) => RedBloodCell = new LabTestResult(value);
	public void SetHemoglobin(string value) => Hemoglobin = new LabTestResult(value);
	public void SetHematocrit(string value) => Hematocrit = new LabTestResult(value);
	public void SetMeanCorpuscularVolume(string value) => MeanCorpuscularVolume = new LabTestResult(value);
	public void SetMeanCorpuscularHemoglobin(string value) => MeanCorpuscularHemoglobin = new LabTestResult(value);
	public void SetMeanCorpuscularHemoglobinConcentration(string value) => MeanCorpuscularHemoglobinConcentration = new LabTestResult(value);
	public void SetRedBloodCellDistributionWidth(string value) => RedBloodCellDistributionWidth = new LabTestResult(value);
	public void SetPlatelet(string value) => Platelet = new LabTestResult(value);
	public void SetMeanPlateletVolume(string value) => MeanPlateletVolume = new LabTestResult(value);
}