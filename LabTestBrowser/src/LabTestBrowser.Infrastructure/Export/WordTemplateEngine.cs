using NPOI.XWPF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class WordTemplateEngine : IWordTemplateEngine
{
	private const string TokenPattern = "{{{{{0}}}}}";

	public Task<MemoryStream> RenderAsync(FileStream fileStream, Dictionary<string, string> tokens)
	{
		using var doc = new XWPFDocument(fileStream);
		using var tempStream = new MemoryStream();

		foreach (var (token, value) in tokens)
		{
			var templateToken = string.Format(TokenPattern, token);
			doc.FindAndReplaceText(templateToken, value);
		}

		doc.Write(tempStream);
		doc.Close();
		tempStream.Position = 0;

		var memoryStream = new MemoryStream(tempStream.ToArray());
		// memoryStream.Position = 0;
		return Task.FromResult(memoryStream);
	}
}