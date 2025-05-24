namespace LabTestBrowser.Urit5160;

public class Urit5160SimulatorSettings
{
	public string Hostname { get; init; } = "localhost";
	public int Port { get; init; } = 4040;
	public int SendingIntervalMs { get; init; } = 5000;
	public int? InitialOrderNumber { get; init; }
}