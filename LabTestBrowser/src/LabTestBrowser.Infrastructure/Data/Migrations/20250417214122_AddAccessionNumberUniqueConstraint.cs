using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessionNumberUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CompleteBloodCounts_AccessionNumber_SequenceNumber_AccessionNumber_Date",
                table: "CompleteBloodCounts",
                columns: new[] { "AccessionNumber_SequenceNumber", "AccessionNumber_Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompleteBloodCounts_AccessionNumber_SequenceNumber_AccessionNumber_Date",
                table: "CompleteBloodCounts");
        }
    }
}
