using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Data.Config;

public class LabTestReportConfiguration : IEntityTypeConfiguration<LabTestReport>
{
	public void Configure(EntityTypeBuilder<LabTestReport> builder)
	{
		builder.OwnsOne(r => r.AccessionNumber);
		builder.OwnsOne(r => r.SpecimenCollectionCenter);
		builder.OwnsOne(r => r.Patient, patient => { patient.OwnsOne(p => p.Age); });
	}
}