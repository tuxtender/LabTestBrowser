using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LabTestBrowser.UI;

public partial class AnimalSpeciesViewModel : ObservableObject
{
	private readonly IReadOnlyCollection<string> _breeds;
	private IReadOnlyCollection<string> _suggestedBreeds = [];

	public AnimalSpeciesViewModel(string animal,
		List<string> breeds,
		List<string> categories)
	{
		Name = animal;
		_breeds = breeds;
		Categories = categories;
	}

	public string Name { get; private set; }

	public IReadOnlyCollection<string> SuggestedBreeds
	{
		get => _suggestedBreeds;
		private set => SetProperty(ref _suggestedBreeds, value);
	}

	public IReadOnlyCollection<string> Categories { get; private set; }

	[RelayCommand]
	private void SuggestBreed(string? text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			SuggestedBreeds = _breeds;
			return;
		}

		var words = text.Split(' ');
		SuggestedBreeds = _breeds
			.Where(breed =>
				words.All(word => breed.Contains(word, StringComparison.CurrentCultureIgnoreCase)))
			.OrderByDescending(breed => breed.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
			.ToList();
	}
}