using System.Windows.Markup;

namespace LabTestBrowser.UI;

public class PositiveIntValidator: MarkupExtension
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return new Func<string, string>(text =>
		{
			return (int.TryParse(text, out int value) && value > 0
				? null
				: "Введите число больше 0")!;
		});
	}
}