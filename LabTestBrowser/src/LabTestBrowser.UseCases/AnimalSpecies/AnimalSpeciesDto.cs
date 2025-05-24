namespace LabTestBrowser.UseCases.AnimalSpecies;

public record AnimalSpeciesDto
{
	public required string Name { get; init; }
	public IEnumerable<string> Categories { get; init; } = [];
	public IEnumerable<string> Breeds { get; init; } = [];
}