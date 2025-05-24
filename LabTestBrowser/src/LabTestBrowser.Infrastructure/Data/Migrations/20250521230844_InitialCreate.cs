using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabTestBrowser.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompleteBloodCounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ExternalId = table.Column<string>(type: "TEXT", nullable: false),
                    ObservationDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AccessionNumber_SequenceNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    AccessionNumber_Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    ReviewResult = table.Column<int>(type: "INTEGER", nullable: false),
                    ReviewDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    WhiteBloodCell_Value = table.Column<string>(type: "TEXT", nullable: true),
                    LymphocytePercent_Value = table.Column<string>(type: "TEXT", nullable: true),
                    MonocytePercent_Value = table.Column<string>(type: "TEXT", nullable: true),
                    NeutrophilPercent_Value = table.Column<string>(type: "TEXT", nullable: true),
                    EosinophilPercent_Value = table.Column<string>(type: "TEXT", nullable: true),
                    BasophilPercent_Value = table.Column<string>(type: "TEXT", nullable: true),
                    RedBloodCell_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Hemoglobin_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Hematocrit_Value = table.Column<string>(type: "TEXT", nullable: true),
                    MeanCorpuscularVolume_Value = table.Column<string>(type: "TEXT", nullable: true),
                    MeanCorpuscularHemoglobin_Value = table.Column<string>(type: "TEXT", nullable: true),
                    MeanCorpuscularHemoglobinConcentration_Value = table.Column<string>(type: "TEXT", nullable: true),
                    RedBloodCellDistributionWidth_Value = table.Column<string>(type: "TEXT", nullable: true),
                    Platelet_Value = table.Column<string>(type: "TEXT", nullable: true),
                    MeanPlateletVolume_Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompleteBloodCounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTestReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccessionNumber_SequenceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    AccessionNumber_Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SpecimenCollectionCenter_Facility = table.Column<string>(type: "TEXT", nullable: false),
                    SpecimenCollectionCenter_TradeName = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_HealthcareProxy = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Name = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Animal = table.Column<string>(type: "TEXT", nullable: false),
                    Patient_Category = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Breed = table.Column<string>(type: "TEXT", nullable: true),
                    Patient_Age_Years = table.Column<int>(type: "INTEGER", nullable: true),
                    Patient_Age_Months = table.Column<int>(type: "INTEGER", nullable: true),
                    Patient_Age_Days = table.Column<int>(type: "INTEGER", nullable: true),
                    Patient_Age_IsEmpty = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestReports", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompleteBloodCounts_AccessionNumber_SequenceNumber_AccessionNumber_Date",
                table: "CompleteBloodCounts",
                columns: new[] { "AccessionNumber_SequenceNumber", "AccessionNumber_Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompleteBloodCounts");

            migrationBuilder.DropTable(
                name: "LabTestReports");
        }
    }
}
