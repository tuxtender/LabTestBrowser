using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterLymField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Lymphocyte_Value",
                table: "CompleteBloodCounts",
                newName: "LymphocytePercent_Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LymphocytePercent_Value",
                table: "CompleteBloodCounts",
                newName: "Lymphocyte_Value");
        }
    }
}
