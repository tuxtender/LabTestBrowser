using CommunityToolkit.Mvvm.ComponentModel;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.UseCases.CompleteBloodCounts;

namespace LabTestBrowser.UI;

public class CompleteBloodCountViewModel : ObservableObject
{
	public CompleteBloodCountViewModel(CompleteBloodCountDto completeBloodCount)
	{
		Id = completeBloodCount.Id;
		PriorityLevel = ConvertToPriorityLevel(completeBloodCount.ReviewResult);
		ExternalId = completeBloodCount.ExternalId!;
		ObservationTimestamp = completeBloodCount.ObservationDateTime;
		Description = Format(completeBloodCount.LabOrderNumber, completeBloodCount.ReviewResult);
		LabOrderNumber = completeBloodCount.LabOrderNumber;
		LabOrderDate = completeBloodCount.LabOrderDate;
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
	public PriorityLevel PriorityLevel { get; init; }
	public string ExternalId { get; init; }
	public DateTime ObservationTimestamp { get; init; }
	public string Description { get; init; }
	public int? LabOrderNumber { get; init; }
	public DateOnly? LabOrderDate { get; init; }
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
			ReviewResult.Suppressed => "Отложен",
			ReviewResult.UnderReview => string.Empty,
			_ => string.Empty
		};
	}

	private static PriorityLevel ConvertToPriorityLevel(ReviewResult reviewResult)
	{
		return reviewResult switch
		{
			ReviewResult.Reported => PriorityLevel.Low,
			ReviewResult.Suppressed => PriorityLevel.Medium,
			ReviewResult.UnderReview => PriorityLevel.High,
			_ => PriorityLevel.None
		};
	}
}