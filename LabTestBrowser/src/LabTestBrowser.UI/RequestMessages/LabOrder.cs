namespace LabTestBrowser.UI;

public class LabOrderViewModel(int number, DateOnly date)
{
	public int Number { get; } = number;
	public DateOnly Date { get; } = date;
}