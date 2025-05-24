namespace LabTestBrowser.UnitTests.Infrastructure.Templating.Engines;

public static class TemplateTestCases
{
	public static IEnumerable<object[]> RenderCases =>
	[
		[
			new TemplateTestCase
			{
				Template = "Multiple: {{a}}, {{b}}",
				Expected = "Multiple: 1, 2",
				Keys =
				[
					"a", "b"
				],
				Values =
				[
					"1", "2"
				]
			}
		],
		[
			new TemplateTestCase
			{
				Template = "Multiple duplicate: {{a}}, {{b}}, {{a}}, {{b}}",
				Expected = "Multiple duplicate: 1, 2, 1, 2",
				Keys =
				[
					"a", "b"
				],
				Values =
				[
					"1", "2"
				]
			}
		]
	];
}