using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.Infrastructure.Data.Config;

public class LabTestReportConfiguration : IEntityTypeConfiguration<LabTestReport>
{
	public void Configure(EntityTypeBuilder<LabTestReport> builder)
	{

		builder.OwnsOne(r => r.Specimen);
		builder.OwnsOne(r => r.SpecimenCollectionCenter);
		builder.OwnsOne(r => r.Patient, patient => 
			{
				patient.OwnsOne(p => p.Age); 
			});
	}

	// modelBuilder.Entity<DetailedOrder>().OwnsOne(
	// 	p => p.OrderDetails, od =>
	// {
	// 	od.WithOwner(d => d.Order);
	// 	od.Navigation(d => d.Order).UsePropertyAccessMode(PropertyAccessMode.Property);
	// 	od.OwnsOne(c => c.BillingAddress);
	// 	od.OwnsOne(c => c.ShippingAddress);
	// });
}