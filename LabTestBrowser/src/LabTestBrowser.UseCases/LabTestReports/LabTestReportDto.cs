﻿namespace LabTestBrowser.UseCases.LabTestReports;

public record LabTestReportDto
{
	public int? Id { get; init; }
	public int OrderNumber { get; init; }
	public DateOnly OrderDate { get; init; }
	public string? Facility { get; init; }
	public string? TradeName { get; init; }
	public string? HealthcareProxy { get; init; }
	public string? Name { get; init; }
	public string? Animal { get; init; }
	public string? Category { get; init; }
	public string? Breed { get; init; }
	public int? AgeInYears { get; init; }
	public int? AgeInMonths { get; init; }
	public int? AgeInDays { get; init; }
}