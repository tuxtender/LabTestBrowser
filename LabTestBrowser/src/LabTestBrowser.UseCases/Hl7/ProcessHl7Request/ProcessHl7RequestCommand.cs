namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public record ProcessHl7RequestCommand(string Message) : ICommand<string>;