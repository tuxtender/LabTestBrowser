using MediatR;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public record CompleteBloodCountCreatedNotification(int Id) : INotification;