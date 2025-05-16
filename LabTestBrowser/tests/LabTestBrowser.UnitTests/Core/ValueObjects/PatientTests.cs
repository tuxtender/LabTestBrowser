using Ardalis.Result;
using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UnitTests.Core.ValueObjects;

public class PatientTests
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void Create_WhenAnimalIsMissing_ReturnsInvalid(string? animal)
	{
		var result = Patient.Create(animal, Age.None, Arg.Any<string?>(),
			Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeFalse();
		result.Status.Should().Be(ResultStatus.Invalid);
	}

	[Fact]
	public void Create_WhenOnlyAnimalIsSet_ReturnsInvalid()
	{
		var animal = nameof(Patient.Animal);

		var result = Patient.Create(animal, Age.None, Arg.Any<string?>(),
			Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeFalse();
		result.Status.Should().Be(ResultStatus.Invalid);
	}

	[Fact]
	public void Create_WhenAnimalIsSetAndAgeIsMissing_ReturnsInvalid()
	{
		var animal = nameof(Patient.Animal);
		var age = Age.None;

		var result = Patient.Create(animal, age, Arg.Any<string?>(),
			Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeFalse();
		result.Status.Should().Be(ResultStatus.Invalid);
	}

	[Fact]
	public void Create_WhenAnimalAndAgeAreSet_ReturnsSuccess()
	{
		var animal = nameof(Patient.Animal);
		var age = Age.Create(1, 2, 3);

		var result = Patient.Create(animal, age, Arg.Any<string?>(),
			Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeTrue();
	}

	[Fact]
	public void Create_WhenAnimalAndHealthcareProxyAreSet_ReturnsSuccess()
	{
		var animal = nameof(Patient.Animal);
		var healthcareProxy = nameof(Patient.HealthcareProxy);

		var result = Patient.Create(animal, Age.None, healthcareProxy,
			Arg.Any<string?>(), Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeTrue();
	}

	[Fact]
	public void Create_WhenAnimalAndNameAreSet_ReturnsSuccess()
	{
		var animal = nameof(Patient.Animal);
		var name = nameof(Patient.Name);

		var result = Patient.Create(animal, Age.None, Arg.Any<string?>(),
			name, Arg.Any<string?>(), Arg.Any<string?>());

		result.IsSuccess.Should().BeTrue();
	}
}