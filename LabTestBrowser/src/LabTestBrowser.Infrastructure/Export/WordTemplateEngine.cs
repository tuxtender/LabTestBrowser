using NPOI.XWPF.UserModel;

namespace LabTestBrowser.Infrastructure.Export;

public class WordTemplateEngine : IWordTemplateEngine
{
	private const string TokenPattern = "{{{{{0}}}}}";

	public Task<MemoryStream> RenderAsync(FileStream fileStream, Dictionary<string, string> tokens)
	{
		var doc = new XWPFDocument(fileStream);

		foreach (var (token, value) in tokens)
		{
			var templateToken = string.Format(TokenPattern, token);
			doc.FindAndReplaceText(templateToken, value);
		}

		var memoryStream = new MemoryStream();
		doc.Write(memoryStream);
		doc.Close();
		memoryStream.Position = 0;

		return Task.FromResult(memoryStream);
	}
}