using LabTestBrowser.UseCases.CompleteBloodCounts.Create;
using LabTestBrowser.UseCases.Hl7.Messaging.v231;
using MediatR;

namespace LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160.SaveUrit5160LabTestResult;

public class SaveUrit5160LabTestResultHandler(IMediator _mediator)
	: ICommandHandler<SaveUrit5160LabTestResultCommand, Result>
{
	public async Task<Result> Handle(SaveUrit5160LabTestResultCommand request, CancellationToken cancellationToken)
	{
		var labTest = Convert(request.OruR01);

		var command = new CreateCompleteBloodCountCommand
		{
			ExternalId = labTest.ExternalId,
			ObservationDateTime = labTest.ObservationDateTime,
			WhiteBloodCell = labTest.WhiteBloodCell,
			LymphocytePercent = labTest.LymphocytePercent,
			MonocytePercent = labTest.MonocytePercent,
			NeutrophilPercent = labTest.NeutrophilPercent,
			EosinophilPercent = labTest.EosinophilPercent,
			BasophilPercent = labTest.BasophilPercent,
			RedBloodCell = labTest.RedBloodCell,
			Hemoglobin = labTest.Hemoglobin,
			Hematocrit = labTest.Hematocrit,
			MeanCorpuscularVolume = labTest.MeanCorpuscularVolume,
			MeanCorpuscularHemoglobin = labTest.MeanCorpuscularHemoglobin,
			MeanCorpuscularHemoglobinConcentration = labTest.MeanCorpuscularHemoglobinConcentration,
			RedBloodCellDistributionWidth = labTest.RedBloodCellDistributionWidth,
			Platelet = labTest.Platelet,
			MeanPlateletVolume = labTest.MeanPlateletVolume,
		};

		await _mediator.Send(command, cancellationToken);

		return Result.Success();
	}

	private static Urit5160LabTestResult Convert(OruR01 oru)
	{
		var labTest = new Urit5160LabTestResult
		{
			ExternalId = oru.Obr.FillerOrderNumber!,
			ObservationDateTime = oru.Obr.ObservationDateTime!.Value,
			WhiteBloodCell = oru.GetObservationValue(Urit5160ObxSegment.WhiteBloodCell),
			LymphocytePercent = oru.GetObservationValue(Urit5160ObxSegment.LymphocytePercent),
			MonocytePercent = oru.GetObservationValue(Urit5160ObxSegment.MonocytePercent),
			NeutrophilPercent = oru.GetObservationValue(Urit5160ObxSegment.NeutrophilPercent),
			EosinophilPercent = oru.GetObservationValue(Urit5160ObxSegment.EosinophilPercent),
			BasophilPercent = oru.GetObservationValue(Urit5160ObxSegment.BasophilPercent),
			RedBloodCell = oru.GetObservationValue(Urit5160ObxSegment.RedBloodCell),
			Hemoglobin = oru.GetObservationValue(Urit5160ObxSegment.Hemoglobin),
			Hematocrit = oru.GetObservationValue(Urit5160ObxSegment.Hematocrit),
			MeanCorpuscularVolume = oru.GetObservationValue(Urit5160ObxSegment.MeanCorpuscularVolume),
			MeanCorpuscularHemoglobin = oru.GetObservationValue(Urit5160ObxSegment.MeanCorpuscularHemoglobin),
			MeanCorpuscularHemoglobinConcentration = oru.GetObservationValue(Urit5160ObxSegment.MeanCorpuscularHemoglobinConcentration),
			RedBloodCellDistributionWidth = oru.GetObservationValue(Urit5160ObxSegment.RedBloodCellDistributionWidth),
			Platelet = oru.GetObservationValue(Urit5160ObxSegment.Platelet),
			MeanPlateletVolume = oru.GetObservationValue(Urit5160ObxSegment.MeanPlateletVolume)
		};

		return labTest;
	}
}