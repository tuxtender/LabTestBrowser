using LabTestBrowser.UseCases.LaboratoryEquipment.Hl7.Dto;

namespace LabTestBrowser.UseCases.LaboratoryEquipment;

public interface IUrit5160Hl7Converter
{
	Task<Urit5160CompleteBloodCount> ConvertAsync(string hl7Message);
}