using Ardalis.Result;
using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UnitTests.Core.ValueObjects;

public class AccessionNumberTests
{
	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void Create_WhenSequenceIsZeroOrNegative_ReturnsInvalid(int sequenceNumber)
	{
		var date = DateOnly.FromDateTime(DateTime.Now);

		var result = AccessionNumber.Create(sequenceNumber, date);

		result.IsSuccess.Should().BeFalse();
		result.Status.Should().Be(ResultStatus.Invalid);
	}
}