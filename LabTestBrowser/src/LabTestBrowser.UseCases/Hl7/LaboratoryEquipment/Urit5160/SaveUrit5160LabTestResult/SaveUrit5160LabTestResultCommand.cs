using LabTestBrowser.UseCases.Hl7.Messaging.v231;

namespace LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160.SaveUrit5160LabTestResult;

public record SaveUrit5160LabTestResultCommand(OruR01 OruR01) : ICommand<Result>;