using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.UseCases.CompleteBloodCounts;

namespace LabTestBrowser.UI;

public class CompleteBloodCountViewModel : BaseViewModel
{
	public CompleteBloodCountViewModel(CompleteBloodCountDto completeBloodCount)
	{
		Id = completeBloodCount.Id;
		ExternalId = completeBloodCount.ExternalId!;
		ObservationTimestamp = completeBloodCount.ObservationDateTime;
		SequenceNumber = Format(completeBloodCount.SequenceNumber, completeBloodCount.ReviewResult);
		WhiteBloodCell = completeBloodCount.WhiteBloodCell ?? string.Empty;
		LymphocytePercent = completeBloodCount.LymphocytePercent ?? string.Empty;
		MonocytePercent = completeBloodCount.MonocytePercent ?? string.Empty;
		EosinophilPercent = completeBloodCount.EosinophilPercent ?? string.Empty;
		RedBloodCell = completeBloodCount.RedBloodCell ?? string.Empty;
		Hemoglobin = completeBloodCount.Hemoglobin ?? string.Empty;
		Hematocrit = completeBloodCount.Hematocrit ?? string.Empty;
		MeanCorpuscularVolume = completeBloodCount.MeanCorpuscularVolume ?? string.Empty;
		MeanCorpuscularHemoglobin = completeBloodCount.MeanCorpuscularHemoglobin ?? string.Empty;
		MeanCorpuscularHemoglobinConcentration = completeBloodCount.MeanCorpuscularHemoglobinConcentration ?? string.Empty;
		RedBloodCellDistributionWidth = completeBloodCount.RedBloodCellDistributionWidth ?? string.Empty;
		Platelet = completeBloodCount.Platelet ?? string.Empty;
	}

	public int? Id { get; set; }
	public string ExternalId { get; init; }
	public DateTime ObservationTimestamp { get; init; }
	public string SequenceNumber { get; init; }
	public string WhiteBloodCell { get; init; }
	public string LymphocytePercent { get; init; }
	public string MonocytePercent { get; init; }
	public string EosinophilPercent { get; init; }
	public string RedBloodCell { get; init; }
	public string Hemoglobin { get; init; }
	public string Hematocrit { get; init; }
	public string MeanCorpuscularVolume { get; init; }
	public string MeanCorpuscularHemoglobin { get; init; }
	public string MeanCorpuscularHemoglobinConcentration { get; init; }
	public string RedBloodCellDistributionWidth { get; init; }
	public string Platelet { get; init; }

	private static string Format(int? sequenceNumber, ReviewResult reviewResult)
	{
		return reviewResult switch
		{
			ReviewResult.Reported => sequenceNumber?.ToString() ?? string.Empty,
			ReviewResult.Suppressed => "X",
			ReviewResult.UnderReview => string.Empty,
			_ => string.Empty
		};
	}
}