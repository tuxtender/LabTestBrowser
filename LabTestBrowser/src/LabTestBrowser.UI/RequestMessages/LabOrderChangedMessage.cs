using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LabTestBrowser.UI.RequestMessages;

public class LabOrderChangedMessage: ValueChangedMessage<LabOrderViewModel>
{
	public LabOrderChangedMessage(LabOrderViewModel labOrder) : base(labOrder)
	{        
	}
}
