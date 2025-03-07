using LabTestBrowser.UI;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

// Build and run the application.
var app = builder.Build();
await app.RunAsync();