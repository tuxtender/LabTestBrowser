namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public record ProcessHl7RequestCommand(byte[] Hl7Message) : ICommand<byte[]>;