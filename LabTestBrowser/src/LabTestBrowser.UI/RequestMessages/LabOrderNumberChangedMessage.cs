using CommunityToolkit.Mvvm.Messaging.Messages;

namespace LabTestBrowser.UI;

public class LabOrderNumberChangedMessage : ValueChangedMessage<int>
{
	public LabOrderNumberChangedMessage(int labOrderNumber) : base(labOrderNumber)
	{        
	}
}