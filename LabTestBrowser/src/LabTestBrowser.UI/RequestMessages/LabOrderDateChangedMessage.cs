using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LabTestBrowser.UI;

public abstract class LabOrderDateChangedMessage(LabOrderViewModel labOrder) : ValueChangedMessage<LabOrderViewModel>(labOrder);