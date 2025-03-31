using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.Infrastructure.Data.Config;

public class CompleteBloodCountConfiguration : IEntityTypeConfiguration<CompleteBloodCount>
{
	public void Configure(EntityTypeBuilder<CompleteBloodCount> builder)
	{
		// builder.Property(x => x.ExternalId).IsRequired();
		// builder.Property(c => c.ObservationDateTime).IsRequired();

		builder.OwnsOne(c => c.WhiteBloodCell);
		builder.OwnsOne(c => c.LymphocytePercent);
		builder.OwnsOne(c => c.MonocytePercent);
		builder.OwnsOne(c => c.NeutrophilPercent);
		builder.OwnsOne(c => c.EosinophilPercent);
		builder.OwnsOne(c => c.BasophilPercent);
		builder.OwnsOne(c => c.RedBloodCell);
		builder.OwnsOne(c => c.Hemoglobin);
		builder.OwnsOne(c => c.Hematocrit);
		builder.OwnsOne(c => c.MeanCorpuscularVolume);
		builder.OwnsOne(c => c.MeanCorpuscularHemoglobin);
		builder.OwnsOne(c => c.MeanCorpuscularHemoglobinConcentration);
		builder.OwnsOne(c => c.RedBloodCellDistributionWidth);
		builder.OwnsOne(c => c.Platelet);
		builder.OwnsOne(c => c.MeanPlateletVolume);
	}
}