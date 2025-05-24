using System.Text.Json;
using LabTestBrowser.Urit5160;

var json = File.ReadAllText("appsettings.json");
var settings = JsonSerializer.Deserialize<Urit5160SimulatorSettings>(json);

var simulator = new Urit5160Simulator(
	settings!.Hostname,
	settings.Port,
	settings.SendingIntervalMs,
	settings.InitialOrderNumber);

await simulator.RunAsync();