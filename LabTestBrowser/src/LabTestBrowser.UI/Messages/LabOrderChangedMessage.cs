using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LabTestBrowser.UI.Messages;

public class LabOrderChangedMessage(LabOrder labOrder) : ValueChangedMessage<LabOrder>(labOrder);
