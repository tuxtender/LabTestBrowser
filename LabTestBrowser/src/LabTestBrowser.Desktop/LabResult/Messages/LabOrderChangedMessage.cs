using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LabTestBrowser.Desktop.LabResult.Messages;

public class LabOrderChangedMessage(LabOrder labOrder) : ValueChangedMessage<LabOrder>(labOrder);
