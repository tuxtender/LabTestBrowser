using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts;

public static class CompleteBloodCountExtensions
{
	public static CompleteBloodCountDto ConvertToCompleteBloodCountDto(this CompleteBloodCount cbc)
	{
		return new CompleteBloodCountDto
		{
			Id = cbc.Id,
			ExternalId = cbc.ExternalId,
			ObservationDateTime = cbc.ObservationDateTime,
			WhiteBloodCell = cbc.WhiteBloodCell?.Value,
			LymphocytePercent = cbc.LymphocytePercent?.Value,
			MonocytePercent = cbc.MonocytePercent?.Value,
			NeutrophilPercent = cbc.NeutrophilPercent?.Value,
			EosinophilPercent = cbc.EosinophilPercent?.Value,
			BasophilPercent = cbc.BasophilPercent?.Value,
			RedBloodCell = cbc.RedBloodCell?.Value,
			Hemoglobin = cbc.Hemoglobin?.Value,
			Hematocrit = cbc.Hematocrit?.Value,
			MeanCorpuscularVolume = cbc.MeanCorpuscularVolume?.Value,
			MeanCorpuscularHemoglobin = cbc.MeanCorpuscularHemoglobin?.Value,
			MeanCorpuscularHemoglobinConcentration = cbc.MeanCorpuscularHemoglobinConcentration?.Value,
			RedBloodCellDistributionWidth = cbc.RedBloodCellDistributionWidth?.Value,
			Platelet = cbc.Platelet?.Value,
			MeanPlateletVolume = cbc.Platelet?.Value,
		};
	}
}