namespace LabTestBrowser.Desktop.LabResult;

public sealed record LabOrderSyncToken
{
	private LabOrderSyncToken(string source) => Source = source;
	public string Source { get; }

	public static readonly LabOrderSyncToken FromPrimary = new(nameof(FromPrimary));
	public static readonly LabOrderSyncToken FromSecondary = new(nameof(FromSecondary));
}