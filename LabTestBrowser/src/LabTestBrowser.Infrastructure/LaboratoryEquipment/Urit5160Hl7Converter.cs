using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.CompleteBloodCount;
using LabTestBrowser.UseCases.LaboratoryEquipment;
using LabTestBrowser.UseCases.LaboratoryEquipment.Hl7.Dto;

namespace LabTestBrowser.Infrastructure.LaboratoryEquipment;

public class Urit5160Hl7Converter(IV231OruR01Converter _converter) : IUrit5160Hl7Converter
{
	public Task<Urit5160CompleteBloodCount> ConvertAsync(string hl7Message)
	{
		var oru = _converter.Convert(hl7Message);

		//TODO: Throw parsing exception
		var cbc = new Urit5160CompleteBloodCount
		{
			ExternalId = oru.Obr.FillerOrderNumber!,
			ObservationDateTime = oru.Obr.ObservationDateTime!.Value,
			WhiteBloodCell = oru.ObxList[Urit5160ObxIndex.WhiteBloodCell].ObservationValue,
			Lymphocyte = oru.ObxList[Urit5160ObxIndex.Lymphocyte].ObservationValue,
			MonocytePercent = oru.ObxList[Urit5160ObxIndex.MonocytePercent].ObservationValue,
			NeutrophilPercent = oru.ObxList[Urit5160ObxIndex.NeutrophilPercent].ObservationValue,
			EosinophilPercent = oru.ObxList[Urit5160ObxIndex.EosinophilPercent].ObservationValue,
			BasophilPercent = oru.ObxList[Urit5160ObxIndex.BasophilPercent].ObservationValue,
			RedBloodCell = oru.ObxList[Urit5160ObxIndex.RedBloodCell].ObservationValue,
			Hemoglobin = oru.ObxList[Urit5160ObxIndex.Hemoglobin].ObservationValue,
			Hematocrit = oru.ObxList[Urit5160ObxIndex.Hematocrit].ObservationValue,
			MeanCorpuscularVolume = oru.ObxList[Urit5160ObxIndex.MeanCorpuscularVolume].ObservationValue,
			MeanCorpuscularHemoglobin = oru.ObxList[Urit5160ObxIndex.MeanCorpuscularHemoglobin].ObservationValue,
			MeanCorpuscularHemoglobinConcentration = oru.ObxList[Urit5160ObxIndex.MeanCorpuscularHemoglobinConcentration].ObservationValue,
			RedBloodCellDistributionWidth = oru.ObxList[Urit5160ObxIndex.RedBloodCellDistributionWidth].ObservationValue,
			Platelet = oru.ObxList[Urit5160ObxIndex.Platelet].ObservationValue,
			MeanPlateletVolume = oru.ObxList[Urit5160ObxIndex.MeanPlateletVolume].ObservationValue,
		};

		return Task.FromResult(cbc);
	}
}