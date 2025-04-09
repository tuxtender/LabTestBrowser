namespace LabTestBrowser.Infrastructure.Data.Settings;

public class AnimalDetails
{
	public int Id { get; init; }
	public required string Title { get; init; }
	public required IEnumerable<string> Genders { get; init; }
	public required IEnumerable<string> Breeds { get; init; }
}