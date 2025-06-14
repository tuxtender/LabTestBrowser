﻿using LabTestBrowser.Core.Common;

namespace LabTestBrowser.Core.LabTestReportAggregate;

public class SpecimenCollectionCenter : ValueObject
{
	private SpecimenCollectionCenter(string facility, string? tradeName)
	{
		Facility = facility;
		TradeName = tradeName;
	}

	public string Facility { get; private set; }
	public string? TradeName { get; private set; }

	public static Result<SpecimenCollectionCenter> Create(string? facility, string? tradeName)
	{
		if (string.IsNullOrWhiteSpace(facility))
			return Result.Invalid(new ValidationError
			{
				ErrorCode = ValidationErrorCode.Required(nameof(SpecimenCollectionCenter), nameof(Facility)).Code
			});

		return new SpecimenCollectionCenter(facility, tradeName);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Facility;
		yield return TradeName ?? string.Empty;
	}
}