using CommunityToolkit.Mvvm.ComponentModel;
using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.UseCases.CompleteBloodCounts;

namespace LabTestBrowser.Desktop.LabResult.CompleteBloodCount;

public class CompleteBloodCountItemViewModel: ObservableObject
{
	private int? _labOrderNumber;
	private DateOnly? _labOrderDate;
	private PriorityLevel _priorityLevel;
	private string _description = string.Empty;

	public CompleteBloodCountItemViewModel(CompleteBloodCountDto completeBloodCount)
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

	public PriorityLevel PriorityLevel
	{
		get => _priorityLevel;
		set => SetProperty(ref _priorityLevel, value);
	}

	public string ExternalId { get; init; }
	public DateTime ObservationTimestamp { get; init; }

	public string Description
	{
		get => _description;
		set => SetProperty(ref _description, value);
	}

	public int? LabOrderNumber
	{
		get => _labOrderNumber;
		set => SetProperty(ref _labOrderNumber, value);
	}

	public DateOnly? LabOrderDate
	{
		get => _labOrderDate;
		set => SetProperty(ref _labOrderDate, value);
	}
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

	public void Update(CompleteBloodCountDto completeBloodCount)
	{
		PriorityLevel = ConvertToPriorityLevel(completeBloodCount.ReviewResult);
		LabOrderNumber = completeBloodCount.LabOrderNumber;
		LabOrderDate = completeBloodCount.LabOrderDate;
		Description = Format(completeBloodCount.LabOrderNumber, completeBloodCount.ReviewResult);
	}

	private static string Format(int? sequenceNumber, ReviewResult reviewResult)
	{
		return reviewResult switch
		{
			ReviewResult.Reported => sequenceNumber?.ToString() ?? string.Empty,
			ReviewResult.Suppressed => "CompleteBloodCount_Suppressed",
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