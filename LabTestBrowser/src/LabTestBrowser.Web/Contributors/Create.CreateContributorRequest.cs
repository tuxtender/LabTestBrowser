using System.ComponentModel.DataAnnotations;

namespace LabTestBrowser.Web.Contributors;

public class CreateContributorRequest
{
	public const string Route = "/Contributors";

	[Required]
	public string? Name { get; set; }

	public string? PhoneNumber { get; set; }
}