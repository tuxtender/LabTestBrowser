using Efferent.HL7.V2;
using NHapiTools.Base.Net;

namespace LabTestBrowser.Urit5160;

public class Urit5160Simulator(string hostname, int port, int sendingIntervalMs, int? initialOrderNumber = null)
{
	private readonly Message _message = new(Urit5160SampleMessage.Hl7Message);

	public async Task RunAsync(CancellationToken cancellationToken = default)
	{
		_message.ParseMessage();
		var orderNumber = initialOrderNumber ?? int.Parse(_message.GetValue("OBR.3"));

		while (!cancellationToken.IsCancellationRequested)
		{
			orderNumber++;

			var formattedOrderNumber = orderNumber.ToString().PadLeft(12, '0');
			_message.SetValue("OBR.3", formattedOrderNumber);
			_message.SetValue("OBR.7", MessageHelper.LongDateWithFractionOfSecond(DateTime.Now));

			try
			{
				using var mllpClient = new SimpleMLLPClient(hostname, port);
				mllpClient.SendHL7Message(_message.SerializeMessage());

				Console.WriteLine("HL7 message sent");
			}
			catch (Exception)
			{
				Console.WriteLine("Sending HL7 message failed");
			}

			await Task.Delay(sendingIntervalMs, cancellationToken);
		}
	}
}