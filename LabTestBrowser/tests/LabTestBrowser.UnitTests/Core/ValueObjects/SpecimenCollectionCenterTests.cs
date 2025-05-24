using Ardalis.Result;
using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UnitTests.Core.ValueObjects;

public class SpecimenCollectionCenterTests
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void Create_WhenFacilityIsNullOrEmpty_ReturnsInvalid(string? facility)
	{
		var result = SpecimenCollectionCenter.Create(facility, Arg.Any<string?>());

		result.IsSuccess.Should().BeFalse();
		result.Status.Should().Be(ResultStatus.Invalid);
	}
}