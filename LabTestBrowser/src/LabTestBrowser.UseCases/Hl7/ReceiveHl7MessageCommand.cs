namespace LabTestBrowser.UseCases.Hl7;

public record ReceiveHl7MessageCommand(string Message) : ICommand<Result>;