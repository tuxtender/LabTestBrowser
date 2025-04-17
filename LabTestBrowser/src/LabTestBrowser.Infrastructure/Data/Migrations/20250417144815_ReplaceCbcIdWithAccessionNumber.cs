using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceCbcIdWithAccessionNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteBloodCountId",
                table: "LabTestReports");

            migrationBuilder.RenameColumn(
                name: "Specimen_Date",
                table: "LabTestReports",
                newName: "AccessionNumber_Date");

            migrationBuilder.RenameColumn(
                name: "Specimen_SequentialNumber",
                table: "LabTestReports",
                newName: "AccessionNumber_SequenceNumber");

            migrationBuilder.AddColumn<DateOnly>(
                name: "AccessionNumber_Date",
                table: "CompleteBloodCounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccessionNumber_SequenceNumber",
                table: "CompleteBloodCounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReviewDate",
                table: "CompleteBloodCounts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewResult",
                table: "CompleteBloodCounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessionNumber_Date",
                table: "CompleteBloodCounts");

            migrationBuilder.DropColumn(
                name: "AccessionNumber_SequenceNumber",
                table: "CompleteBloodCounts");

            migrationBuilder.DropColumn(
                name: "ReviewDate",
                table: "CompleteBloodCounts");

            migrationBuilder.DropColumn(
                name: "ReviewResult",
                table: "CompleteBloodCounts");

            migrationBuilder.RenameColumn(
                name: "AccessionNumber_Date",
                table: "LabTestReports",
                newName: "Specimen_Date");

            migrationBuilder.RenameColumn(
                name: "AccessionNumber_SequenceNumber",
                table: "LabTestReports",
                newName: "Specimen_SequentialNumber");

            migrationBuilder.AddColumn<int>(
                name: "CompleteBloodCountId",
                table: "LabTestReports",
                type: "INTEGER",
                nullable: true);
        }
    }
}
